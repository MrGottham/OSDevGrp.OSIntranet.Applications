using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Core.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Mvc.Tests
{
    [TestFixture]
    public class DataImporter
    {
        #region Private variables

        private ICommandBus _commandBus;
        private IQueryBus _queryBus;

        #endregion

        [SetUp]
        public void SetUp()
        {
            IConfiguration configuration = CreateConfiguration();
            IPrincipalResolver principalResolver = CreatePrincipalResolver();
            ILoggerFactory loggerFactory = CreateLoggerFactory();
            IValidator validator = CreateValidator();

            IClaimResolver claimResolver = new ClaimResolver(principalResolver);
            ICountryHelper countryHelper = new CountryHelper(claimResolver);

            ICommonRepository commonRepository = new CommonRepository(configuration, principalResolver, loggerFactory);
            IContactRepository contactRepository = new ContactRepository(configuration, principalResolver, loggerFactory);

            ICommandHandler<ICreateLetterHeadCommand> createLetterHeadCommandHandler = new CreateLetterHeadCommandHandler(validator, commonRepository);
            ICommandHandler<ICreatePostalCodeCommand> createPostalCodeCommand = new CreatePostalCodeCommandHandler(validator, contactRepository);
            _commandBus = new CommandBus(new ICommandHandler[] {createLetterHeadCommandHandler, createPostalCodeCommand});

            IQueryHandler<EmptyQuery, IEnumerable<ICountry>> getCountryCollectionQueryHandler = new GetCountryCollectionQueryHandler(contactRepository, countryHelper);
            IQueryHandler<IGetPostalCodeQuery, IPostalCode> getPostalCodeQueryHandler = new GetPostalCodeQueryHandler(validator, contactRepository, countryHelper);
            _queryBus = new QueryBus(new IQueryHandler[] {getCountryCollectionQueryHandler, getPostalCodeQueryHandler});
        }

        [Test]
        [Category("DataImport")]
        [TestCase("LetterHeads.csv")]
        [Ignore("Test which imports data should only be run once")]
        public async Task Import_LetterHeads_FromFile(string fileName)
        {
            await ImportFromFile(fileName, Encoding.UTF7, async (lineNumber, line) =>
            {
                if (lineNumber <= 1)
                {
                    return;
                }

                string[] values = CsvSplitter(line);

                ICreateLetterHeadCommand command = new CreateLetterHeadCommand
                {
                    Number = int.Parse(values[0]),
                    Name = values[1],
                    Line1 = values[4],
                    Line2 = string.IsNullOrWhiteSpace(values[5]) ? null : values[5],
                    Line3 = string.IsNullOrWhiteSpace(values[6]) ? null : values[6],
                    Line4 = string.IsNullOrWhiteSpace(values[7]) ? null : values[7],
                    Line5 = string.IsNullOrWhiteSpace(values[8]) ? null : values[8],
                    Line6 = string.IsNullOrWhiteSpace(values[9]) ? null : values[9],
                    Line7 = string.IsNullOrWhiteSpace(values[10]) ? null : values[10],
                    CompanyIdentificationNumber = string.IsNullOrWhiteSpace(values[11]) ? null : values[11],
                };
                await _commandBus.PublishAsync(command);
            });
        }

        [Test]
        [Category("DataImport")]
        [TestCase("PostalCodes.xml")]
        [Ignore("Test which imports data should only be run once")]
        public async Task Import_PostalCodes_FromFile(string fileName)
        {
            XmlDocument postalCodeDocument = new XmlDocument();
            postalCodeDocument.Load(fileName);

            IDictionary<string, ICountry> countryDictionary = (await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(new EmptyQuery())).ToDictionary(country => country.Code, country => country);

            XmlNodeList postalCodeNodeList = postalCodeDocument.DocumentElement.SelectNodes("PostalCode");
            foreach (XmlElement postalCodeElement in postalCodeNodeList.OfType<XmlElement>())
            {
                string countryCode = GetAttributeValue(postalCodeElement, "countryCode");
                if (string.IsNullOrWhiteSpace(countryCode) || countryDictionary.ContainsKey(countryCode) == false)
                {
                    continue;
                }

                string code = GetAttributeValue(postalCodeElement, "code");
                if (string.IsNullOrWhiteSpace(code))
                {
                    continue;
                }

                IGetPostalCodeQuery query = new GetPostalCodeQuery
                {
                    CountryCode = countryCode,
                    PostalCode = code
                };
                if (await _queryBus.QueryAsync<IGetPostalCodeQuery, IPostalCode>(query) != null)
                {
                    continue;
                }

                ICreatePostalCodeCommand command = new CreatePostalCodeCommand
                {
                    CountryCode = countryCode,
                    PostalCode = code,
                    City = GetAttributeValue(postalCodeElement, "city"),
                    State = GetAttributeValue(postalCodeElement, "state")
                };
                await _commandBus.PublishAsync(command);
            }
        }

        private IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<DataImporter>()
                .Build();
        }

        private IPrincipalResolver CreatePrincipalResolver()
        {
            IPrincipal currentPrincipal = CreateClaimsPrincipal();
            return new GenericPrincipalResolver(currentPrincipal);
        }

        private ILoggerFactory CreateLoggerFactory()
        {
            return NullLoggerFactory.Instance;
        }

        private ClaimsPrincipal CreateClaimsPrincipal()
        {
            Claim nameClaim = new Claim(ClaimTypes.Name, "OSDevGrp.OSIntranet.Repositories.Migrations");
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new [] {nameClaim});

            return new ClaimsPrincipal(claimsIdentity);
        }

        private IValidator CreateValidator()
        {
            return new Validator(new IntegerValidator(), new DecimalValidator(), new StringValidator(), new DateTimeValidator(), new ObjectValidator());
        }

        private async Task ImportFromFile(string fileName, Encoding encoding, Func<int, string, Task> lineHandler)
        {
            NullGuard.NotNullOrWhiteSpace(fileName, nameof(fileName))
                .NotNull(encoding, nameof(encoding))
                .NotNull(lineHandler, nameof(lineHandler));

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, encoding))
                {
                    int lineNumber = 0;
                    while (streamReader.EndOfStream == false)
                    {
                        await lineHandler(++lineNumber, streamReader.ReadLine());
                    }
                }
            }
        }

        private string[] CsvSplitter(string line)
        {
            NullGuard.NotNullOrWhiteSpace(line, nameof(line));

            return line.Split(';')
                .Select(value => 
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }

                    if (value.StartsWith('"') && value.EndsWith('"'))
                    {
                        return value.Substring(1, value.Length - 2);
                    }

                    return value;
                })
                .ToArray();
        }

        private string GetAttributeValue(XmlElement element, string attributeName)
        {
            NullGuard.NotNull(element, nameof(element))
                .NotNullOrWhiteSpace(attributeName, nameof(attributeName));

            XmlAttribute attribute = element.GetAttributeNode(attributeName);
            if (attribute == null || string.IsNullOrWhiteSpace(attribute.Value))
            {
                return null;
            }

            return attribute.Value.Trim();
        }
    }
}