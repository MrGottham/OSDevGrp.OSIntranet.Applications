using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Common.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Core.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Mvc.Tests
{
    [TestFixture]
    public class DataImporter
    {
        #region Private variables

        private IValidator _validator;
        private IMicrosoftGraphRepository _microsoftGraphRepository;
        private IAccountingRepository _accountingRepository;
        private IContactRepository _contactRepository;
        private ICommandBus _commandBus;
        private IQueryBus _queryBus;

        private static readonly Regex PersonNameRegex = new Regex(@"^([A-Z∆ÿ≈‹»][A-Z∆ÿ≈‹»a-zÊ¯Â¸È\-]+\s)?([A-Z∆ÿ≈‹»][A-Z∆ÿ≈‹»a-zÊ¯Â¸È.\s]+\s)?([A-Z∆ÿ≈‹»][A-Z∆ÿ≈‹»a-zÊ¯Â¸È\-]+)$", RegexOptions.Compiled);
        private static readonly Regex DanishPostalCodeAndCityRegex = new Regex(@"^([0-9]{4})\s+([A-Z∆ÿ≈][A-Z∆ÿ≈a-zÊ¯Â\s]*)$", RegexOptions.Compiled);
        private static readonly Regex DanishPhoneNumberRegex = new Regex(@"^(\+45)?[\s]*([0-9]{2})[\s]*([0-9]{2})[\s]*([0-9]{2})[\s]*([0-9]{2})$", RegexOptions.Compiled);
        private static readonly Regex UnitedStatesPostalCodeAndCityRegex = new Regex(@"^([A-Z][a-z]+),\s+([A-Z]+)\s+([0-9]{5})$", RegexOptions.Compiled);
        private static readonly Regex GermanyPostalCodeAndCityRegex = new Regex(@"^D-([0-9]{5})\s+([A-Z][a-z]+)$", RegexOptions.Compiled);
        private static readonly Regex GermanyPhoneNumberRegex = new Regex(@"^\+49[\s]*([0-9]{2})[\s]*([0-9]{8})$", RegexOptions.Compiled);

        #endregion

        [SetUp]
        public void SetUp()
        {
            IConfiguration configuration = CreateConfiguration();
            IPrincipalResolver principalResolver = CreatePrincipalResolver();
            ILoggerFactory loggerFactory = CreateLoggerFactory();
            
            _validator = CreateValidator();

            IClaimResolver claimResolver = new ClaimResolver(principalResolver);
            ICountryHelper countryHelper = new CountryHelper(claimResolver);
            IAccountingHelper accountingHelper = new AccountingHelper(claimResolver);

            ICommonRepository commonRepository = new CommonRepository(configuration, principalResolver, loggerFactory);

            _microsoftGraphRepository = new MicrosoftGraphRepository(configuration, principalResolver, loggerFactory);
            _contactRepository = new ContactRepository(configuration, principalResolver, loggerFactory);
            _accountingRepository = new AccountingRepository(configuration, principalResolver, loggerFactory);

            ICommandHandler<IRefreshTokenForMicrosoftGraphCommand, IRefreshableToken> refreshTokenForMicrosoftGraphCommandHandler = new RefreshTokenForMicrosoftGraphCommandHandler(_microsoftGraphRepository);
            ICommandHandler<ICreateLetterHeadCommand> createLetterHeadCommandHandler = new CreateLetterHeadCommandHandler(_validator, commonRepository);
            ICommandHandler<ICreateContactCommand> createContactCommandHandler = new CreateContactCommandHandler(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);
            ICommandHandler<IUpdateContactCommand> updateContactCommandHandler = new UpdateContactCommandHandler(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);
            ICommandHandler<ICreateContactGroupCommand> createContactGroupCommandHandler = new CreateContactGroupCommandHandler(_validator, _contactRepository);
            ICommandHandler<ICreatePostalCodeCommand> createPostalCodeCommandHandler = new CreatePostalCodeCommandHandler(_validator, _contactRepository);
            ICommandHandler<ICreateAccountingCommand> createAccountingCommandHandler = new CreateAccountingCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<IUpdateAccountingCommand> updateAccountingCommandHandler = new UpdateAccountingCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<ICreatePaymentTermCommand> createPaymentTermCommandHandler = new CreatePaymentTermCommandHandler(_validator, _accountingRepository);
            _commandBus = new CommandBus(new ICommandHandler[]
            {
                refreshTokenForMicrosoftGraphCommandHandler,
                createLetterHeadCommandHandler,
                createContactCommandHandler,
                updateContactCommandHandler,
                createContactGroupCommandHandler,
                createPostalCodeCommandHandler, 
                createAccountingCommandHandler,
                updateAccountingCommandHandler,
                createPaymentTermCommandHandler
            });

            IQueryHandler<IGetMatchingContactCollectionQuery, IEnumerable<IContact>> getMatchingContactCollectionQueryHandler = new GetMatchingContactCollectionQueryHandler(_validator, _microsoftGraphRepository, _contactRepository);
            IQueryHandler<EmptyQuery, IEnumerable<ICountry>> getCountryCollectionQueryHandler = new GetCountryCollectionQueryHandler(_contactRepository, countryHelper);
            IQueryHandler<IGetPostalCodeQuery, IPostalCode> getPostalCodeQueryHandler = new GetPostalCodeQueryHandler(_validator, _contactRepository, countryHelper);
            IQueryHandler<EmptyQuery, IEnumerable<IAccounting>> getAccountingCollectionQueryHandler = new GetAccountingCollectionQueryHandler(_accountingRepository, accountingHelper);
            _queryBus = new QueryBus(new IQueryHandler[]
            {
                getMatchingContactCollectionQueryHandler,
                getCountryCollectionQueryHandler,
                getPostalCodeQueryHandler,
                getAccountingCollectionQueryHandler
            });
        }

        [Test]
        [Category("DataImport")]
        [TestCase("LetterHeads.csv")]
        [Ignore("Test which imports data and should only be run once")]
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
        [TestCase("ContactGroups.xml")] 
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_ContactGroups_FromFile(string fileName)
        {
            XmlDocument contactGroupDocument = new XmlDocument();
            contactGroupDocument.Load(fileName);

            XmlNodeList contactGroupNodeList = contactGroupDocument.DocumentElement.SelectNodes("ContactGroup");
            foreach (XmlElement contactGroupElement in contactGroupNodeList.OfType<XmlElement>())
            {
                if (int.TryParse(contactGroupElement.GetAttribute("number"), out int contactGroupNumber) == false)
                {
                    continue;
                }

                string contactGroupName = contactGroupElement.GetAttribute("name");
                if (string.IsNullOrWhiteSpace(contactGroupName))
                {
                    continue;
                }

                ICreateContactGroupCommand createContactGroupCommand = new CreateContactGroupCommand
                {
                    Number = contactGroupNumber,
                    Name = contactGroupName
                };
                await _commandBus.PublishAsync(createContactGroupCommand);
            }
        }

        [Test]
        [Category("DataImport")]
        [TestCase("PostalCodes.xml")]
        [Ignore("Test which imports data and should only be run once")]
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

        [Test]
        [Category("DataImport")]
        [TestCase("Accountings.xml")] 
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_Accountings_FromFile(string fileName)
        {
            XmlDocument accountingDocument = new XmlDocument();
            accountingDocument.Load(fileName);

            IDictionary<int, IAccounting> accountingDictionary = (await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(new EmptyQuery())).ToDictionary(accounting => accounting.Number, accounting => accounting);

            XmlNodeList accountingNodeList = accountingDocument.DocumentElement.SelectNodes("Accounting");
            foreach (XmlElement accountingElement in accountingNodeList.OfType<XmlElement>())
            {
                if (int.TryParse(accountingElement.GetAttribute("number"), out int accountingNumber) == false)
                {
                    continue;
                }

                string accountingName = accountingElement.GetAttribute("name");
                if (string.IsNullOrWhiteSpace(accountingName))
                {
                    continue;
                }

                if (int.TryParse(accountingElement.GetAttribute("letterHeadNumber"), out int letterHeadNumber) == false)
                {
                    continue;
                }

                if (accountingDictionary.ContainsKey(accountingNumber))
                {
                    IUpdateAccountingCommand updateAccountingCommand = new UpdateAccountingCommand
                    {
                        AccountingNumber = accountingNumber,
                        Name = accountingName,
                        LetterHeadNumber = letterHeadNumber,
                        BalanceBelowZero = accountingDictionary[accountingNumber].BalanceBelowZero,
                        BackDating = accountingDictionary[accountingNumber].BackDating
                    };
                    await _commandBus.PublishAsync(updateAccountingCommand);
                    continue;
                }

                ICreateAccountingCommand createAccountingCommand = new CreateAccountingCommand
                {
                    AccountingNumber = accountingNumber,
                    Name = accountingName,
                    LetterHeadNumber = letterHeadNumber,
                    BalanceBelowZero = BalanceBelowZeroType.Debtors,
                    BackDating = 30
                };
                await _commandBus.PublishAsync(createAccountingCommand);
            }
        }

        [Test]
        [Category("DataImport")]
        [TestCase(@"PaymentTerms.xml")]
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_PaymentTerms_FromFile(string fileName)
        {
            XmlDocument paymentTermDocument = new XmlDocument();
            paymentTermDocument.Load(fileName);

            XmlNodeList paymentTermNodeList = paymentTermDocument.DocumentElement.SelectNodes("PaymentTerm");
            foreach (XmlElement paymentTermElement in paymentTermNodeList.OfType<XmlElement>())
            {
                if (int.TryParse(paymentTermElement.GetAttribute("number"), out int paymentTermNumber) == false)
                {
                    continue;
                }

                string paymentTermName = paymentTermElement.GetAttribute("name");
                if (string.IsNullOrWhiteSpace(paymentTermName))
                {
                    continue;
                }

                ICreatePaymentTermCommand createPaymentTermCommand = new CreatePaymentTermCommand
                {
                    Number = paymentTermNumber,
                    Name = paymentTermName
                };
                await _commandBus.PublishAsync(createPaymentTermCommand);
            }
        }

        [Test]
        [Category("DataImport")]
        [TestCase(@"Addresses.xml", "[TBD]", "[TBD]", "[TBD]", "[TBD]")]
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_Addresses_FromFile(string fileName, string tokenType, string accessToken, string expires, string refreshToken)
        {
            const bool validateCommands = false;
            DateTime expiresDateTime = DateTime.Parse(expires, CultureInfo.InvariantCulture);

            IDictionary<int, XmlElement> elementDictionary = new Dictionary<int, XmlElement>();

            XmlDocument addressDocument = new XmlDocument();
            addressDocument.Load(fileName);

            XmlNodeList companyNodeList = addressDocument.DocumentElement.SelectNodes("Company");
            foreach (XmlElement companyElement in companyNodeList.OfType<XmlElement>())
            {
                int number = int.Parse(GetAttributeValue(companyElement, "number"));

                elementDictionary.Add(number, companyElement);
            }

            XmlNodeList personNodeList = addressDocument.DocumentElement.SelectNodes("Person");
            foreach (XmlElement personElement in personNodeList.OfType<XmlElement>())
            {
                int number = int.Parse(GetAttributeValue(personElement, "number"));

                elementDictionary.Add(number, personElement);
            }

            foreach (XmlElement element in elementDictionary.OrderBy(m => m.Key).Select(m => m.Value).ToArray())
            {
                if (expiresDateTime <= DateTime.Now)
                {
                    IRefreshTokenForMicrosoftGraphCommand command = new RefreshTokenForMicrosoftGraphCommand(new Uri("[TBD]"), new RefreshableToken(tokenType, accessToken, refreshToken, expiresDateTime.ToUniversalTime()));
                    IRefreshableToken token = await _commandBus.PublishAsync<IRefreshTokenForMicrosoftGraphCommand, IRefreshableToken>(command);

                    tokenType = token.TokenType;
                    accessToken = token.AccessToken;
                    expiresDateTime = token.Expires.ToLocalTime();
                    refreshToken = token.RefreshToken;
                }

                IContact contact = await FindContactAsync(tokenType, accessToken, expiresDateTime, refreshToken, element);

                if (contact == null)
                {
                    ICreateContactCommand createContactCommand;
                    switch (element.LocalName)
                    {
                        case "Person":
                            createContactCommand = BuildCreateContactCommandForPerson(tokenType, accessToken, expiresDateTime, refreshToken, element, elementDictionary, validateCommands);
                            break;

                        case "Company":
                            createContactCommand = BuildCreateContactCommandForCompany(tokenType, accessToken, expiresDateTime, refreshToken, element, validateCommands);
                            break;

                        default:
                            throw new NotSupportedException($"Unhandled element: {element.OuterXml}");
                    }
                    await _commandBus.PublishAsync(createContactCommand);
                    continue;
                }

                IUpdateContactCommand updateContactCommand;
                switch (element.LocalName)
                {
                    case "Person":
                        updateContactCommand = BuildUpdateContactCommandForPerson(tokenType, accessToken, expiresDateTime, refreshToken, element, contact, elementDictionary, validateCommands);
                        break;

                    case "Company":
                        updateContactCommand = BuildUpdateContactCommandForCompany(tokenType, accessToken, expiresDateTime, refreshToken, element, contact, validateCommands);
                        break;

                    default:
                        throw new NotSupportedException($"Unhandled element: {element.OuterXml}");
                }
                await _commandBus.PublishAsync(updateContactCommand);
            }
        }

        private async Task<IContact> FindContactAsync(string tokenType, string accessToken, DateTime expires, string refreshToken, XmlElement element)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken))
                .NotNullOrWhiteSpace(refreshToken, nameof(refreshToken))
                .NotNull(element, nameof(element));

            string name = GetAttributeValue(element, "name");
            string mailAddress = GetAttributeValue(element, "mailAddress");
            string primaryPhone = GetAttributeValue(element, "primaryPhone");
            string secondaryPhone = GetAttributeValue(element, "secondaryPhone");
            string homePhone = GetAttributeValue(element, "homePhone");
            string mobilePhone = GetAttributeValue(element, "mobilePhone");

            if (string.IsNullOrWhiteSpace(name) == false)
            {
                IContact matchingContact = await FindContactAsync(tokenType, accessToken, expires, refreshToken, query => { query.SearchFor = name; query.SearchWithinName = true; });
                if (matchingContact != null)
                {
                    return matchingContact;
                }
            }

            if (string.IsNullOrWhiteSpace(mailAddress) == false)
            {
                IContact matchingContact = await FindContactAsync(tokenType, accessToken, expires, refreshToken, query => { query.SearchFor = mailAddress; query.SearchWithinMailAddress = true; });
                if (matchingContact != null)
                {
                    return matchingContact;
                }
            }

            if (string.IsNullOrWhiteSpace(name) == false && string.IsNullOrWhiteSpace(mailAddress) && string.IsNullOrWhiteSpace(primaryPhone) && string.IsNullOrWhiteSpace(secondaryPhone) && string.IsNullOrWhiteSpace(homePhone) && string.IsNullOrWhiteSpace(mobilePhone))
            {
                IContact matchingContact = await FindContactAsync(tokenType, accessToken, expires, refreshToken, query => { query.SearchFor = name; query.SearchWithinName = true; }, contact => string.IsNullOrWhiteSpace(contact.MailAddress) && string.IsNullOrWhiteSpace(contact.PrimaryPhone) && string.IsNullOrWhiteSpace(contact.SecondaryPhone) && string.IsNullOrWhiteSpace(contact.HomePhone) && string.IsNullOrWhiteSpace(contact.MobilePhone));
                if (matchingContact != null)
                {
                    return matchingContact;
                }
            }

            return null;
        }

        private async Task<IContact> FindContactAsync(string tokenType, string accessToken, DateTime expires, string refreshToken, Action<IGetMatchingContactCollectionQuery> querySetter, Func<IContact, bool> predicate = null)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken))
                .NotNullOrWhiteSpace(refreshToken, nameof(refreshToken))
                .NotNull(querySetter, nameof(querySetter));

            IGetMatchingContactCollectionQuery query = new GetMatchingContactCollectionQuery
            {
                TokenType = tokenType,
                AccessToken = accessToken,
                Expires = expires,
                RefreshToken = refreshToken
            };
            querySetter(query);

            IContact[] matchingContactCollection = (await _queryBus.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(query))
                .Where(contact => predicate == null || predicate(contact))
                .ToArray();
            
            return matchingContactCollection.Length == 1 ? matchingContactCollection.First() : null;
        }

        private ICreateContactCommand BuildCreateContactCommandForCompany(string tokenType, string accessToken, DateTime expires, string refreshToken, XmlElement companyElement, bool validate)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken))
                .NotNullOrWhiteSpace(refreshToken, nameof(refreshToken))
                .NotNull(companyElement, nameof(companyElement));

            ICreateContactCommand command = new CreateContactCommand
            {
                TokenType = tokenType,
                AccessToken = accessToken,
                Expires = expires,
                RefreshToken = refreshToken,
                Name = BuildNameCommandForCompany(companyElement)
            };

            command = FillContactDataCommandForCompany(companyElement, command);

            if (validate == false)
            {
                return command;
            }
            
            command.Validate(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);

            return command;
        }

        private IUpdateContactCommand BuildUpdateContactCommandForCompany(string tokenType, string accessToken, DateTime expires, string refreshToken, XmlElement companyElement, IContact matchingContact, bool validate)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken))
                .NotNullOrWhiteSpace(refreshToken, nameof(refreshToken))
                .NotNull(companyElement, nameof(companyElement))
                .NotNull(matchingContact, nameof(matchingContact));

            IUpdateContactCommand command = new UpdateContactCommand
            {
                TokenType = tokenType,
                AccessToken = accessToken,
                Expires = expires,
                RefreshToken = refreshToken,
                ExternalIdentifier = matchingContact.ExternalIdentifier,
                Name = BuildNameCommandForCompany(companyElement)
            };

            command = FillContactDataCommandForCompany(companyElement, command);

            if (validate == false)
            {
                return command;
            }
            
            command.Validate(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);

            return command;
        }

        private INameCommand BuildNameCommandForCompany(XmlElement companyElement)
        {
            NullGuard.NotNull(companyElement, nameof(companyElement));

            return new CompanyNameCommand
            {
                FullName = GetAttributeValue(companyElement, "name")
            };
        }

        private T FillContactDataCommandForCompany<T>(XmlElement companyElement, T contactDataCommand) where T : IContactDataCommand
        {
            NullGuard.NotNull(companyElement, nameof(companyElement))
                .NotNull(contactDataCommand, nameof(contactDataCommand));

            string primaryPhone = GetAttributeValue(companyElement, "primaryPhone");
            string secondaryPhone = GetAttributeValue(companyElement, "secondaryPhone");
            if (string.IsNullOrWhiteSpace(primaryPhone) == false && string.IsNullOrWhiteSpace(secondaryPhone) == false)
            {
                contactDataCommand.HomePhone = BuildPhoneNumber(primaryPhone);
                if (string.Compare(contactDataCommand.HomePhone, BuildPhoneNumber(secondaryPhone), StringComparison.InvariantCulture) != 0)
                {
                    contactDataCommand.MobilePhone = BuildPhoneNumber(secondaryPhone);
                }
            }
            else if (string.IsNullOrWhiteSpace(primaryPhone) == false)
            {
                contactDataCommand.HomePhone = BuildPhoneNumber(primaryPhone);
                contactDataCommand.MobilePhone = null;
            }
            else if (string.IsNullOrWhiteSpace(secondaryPhone) == false)
            {
                contactDataCommand.HomePhone = BuildPhoneNumber(secondaryPhone);
                contactDataCommand.MobilePhone = null;
            }

            return FillContactDataCommand(companyElement, contactDataCommand);
        }

        private ICreateContactCommand BuildCreateContactCommandForPerson(string tokenType, string accessToken, DateTime expires, string refreshToken, XmlElement personElement, IDictionary<int, XmlElement> elementDictionary, bool validate)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken))
                .NotNullOrWhiteSpace(refreshToken, nameof(refreshToken))
                .NotNull(personElement, nameof(personElement))
                .NotNull(elementDictionary, nameof(elementDictionary));

            ICreateContactCommand command = new CreateContactCommand
            {
                TokenType = tokenType,
                AccessToken = accessToken,
                Expires = expires,
                RefreshToken = refreshToken,
                Name = BuildNameCommandForPerson(personElement)
            };

            command = FillContactDataCommandForPerson(personElement, elementDictionary, command);

            if (validate == false)
            {
                return command;
            }

            command.Validate(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);

            return command;
        }

        private IUpdateContactCommand BuildUpdateContactCommandForPerson(string tokenType, string accessToken, DateTime expires, string refreshToken, XmlElement personElement, IContact matchingContact, IDictionary<int, XmlElement> elementDictionary, bool validate)
        {
            NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
                .NotNullOrWhiteSpace(accessToken, nameof(accessToken))
                .NotNullOrWhiteSpace(refreshToken, nameof(refreshToken))
                .NotNull(personElement, nameof(personElement))
                .NotNull(matchingContact, nameof(matchingContact))
                .NotNull(elementDictionary, nameof(elementDictionary));

            IUpdateContactCommand command = new UpdateContactCommand
            {
                TokenType = tokenType,
                AccessToken = accessToken,
                Expires = expires,
                RefreshToken = refreshToken,
                ExternalIdentifier = matchingContact.ExternalIdentifier,
                Name = BuildNameCommandForPerson(personElement)
            };

            command = FillContactDataCommandForPerson(personElement, elementDictionary, command);

            if (validate == false)
            {
                return command;
            }
            
            command.Validate(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);

            return command;
        }

        private INameCommand BuildNameCommandForPerson(XmlElement personElement)
        {
            NullGuard.NotNull(personElement, nameof(personElement));

            string personName = GetAttributeValue(personElement, "name");

            Match personNameMatch = PersonNameRegex.Match(personName);
            if (personNameMatch.Success == false)
            {
                throw new NotSupportedException($"The value does not match the person name format: {personName}");
            }

            return new PersonNameCommand
            {
                GivenName = string.IsNullOrWhiteSpace(personNameMatch.Groups[1].Value) ? null : personNameMatch.Groups[1].Value,
                MiddleName = string.IsNullOrWhiteSpace(personNameMatch.Groups[2].Value) ? null : personNameMatch.Groups[2].Value,
                Surname = string.IsNullOrWhiteSpace(personNameMatch.Groups[3].Value) ? null : personNameMatch.Groups[3].Value
            };
        }

        private T FillContactDataCommandForPerson<T>(XmlElement personElement, IDictionary<int, XmlElement> elementDictionary, T contactDataCommand) where T : IContactDataCommand
        {
            NullGuard.NotNull(personElement, nameof(personElement))
                .NotNull(elementDictionary, nameof(elementDictionary))
                .NotNull(contactDataCommand, nameof(contactDataCommand));

            string homePhone = GetAttributeValue(personElement, "homePhone");
            string mobilePhone = GetAttributeValue(personElement, "mobilePhone");
            if (string.IsNullOrWhiteSpace(homePhone) == false && string.IsNullOrWhiteSpace(mobilePhone) == false)
            {
                contactDataCommand.MobilePhone = BuildPhoneNumber(mobilePhone);
                if (string.Compare(contactDataCommand.MobilePhone, BuildPhoneNumber(homePhone), StringComparison.InvariantCulture) != 0)
                {
                    contactDataCommand.HomePhone = BuildPhoneNumber(homePhone);
                }
            }
            else if (string.IsNullOrWhiteSpace(homePhone) == false)
            {
                contactDataCommand.HomePhone = BuildPhoneNumber(homePhone);
                contactDataCommand.MobilePhone = null;
            }
            else if (string.IsNullOrWhiteSpace(mobilePhone) == false)
            {
                contactDataCommand.MobilePhone = BuildPhoneNumber(mobilePhone);
                contactDataCommand.HomePhone = null;
            }

            string birthdayAsString = GetAttributeValue(personElement, "birthday");
            if (string.IsNullOrWhiteSpace(birthdayAsString) == false)
            {
                contactDataCommand.Birthday = DateTime.ParseExact(birthdayAsString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            contactDataCommand.Company = BuildCompanyCommand(personElement, elementDictionary);

            return FillContactDataCommand(personElement, contactDataCommand);
        }

        private ICompanyCommand BuildCompanyCommand(XmlElement personElement, IDictionary<int, XmlElement> elementDictionary)
        {
            NullGuard.NotNull(personElement, nameof(personElement))
                .NotNull(elementDictionary, nameof(elementDictionary));

            string companyIdentifierAsString = GetAttributeValue(personElement, "companyIdentifier");
            if (string.IsNullOrWhiteSpace(companyIdentifierAsString))
            {
                return null;
            }

            int companyIdentifier = int.Parse(companyIdentifierAsString);
            if (elementDictionary.ContainsKey(companyIdentifier) == false)
            {
                throw new NotSupportedException($"The value does not match any known companies: {companyIdentifier}");
            }

            IContactDataCommand companyContactData = BuildCreateContactCommandForCompany("[TBD]", "[TBD]", DateTime.Now.AddHours(1), "[TBD]", elementDictionary[companyIdentifier], false);

            return new CompanyCommand
            {
                Name = (ICompanyNameCommand) companyContactData.Name,
                Address = companyContactData.Address,
                PrimaryPhone = companyContactData.HomePhone,
                SecondaryPhone = companyContactData.MobilePhone,
                HomePage = companyContactData.PersonalHomePage,
            };
        }

        private T FillContactDataCommand<T>(XmlElement element, T contactDataCommand) where T : IContactDataCommand
        {
            NullGuard.NotNull(element, nameof(element))
                .NotNull(contactDataCommand, nameof(contactDataCommand));

            contactDataCommand.Address = BuildAddressCommand(element);
            contactDataCommand.MailAddress = BuildMailAddress(element); 
            contactDataCommand.ContactGroupIdentifier = int.Parse(GetAttributeValue(element, "contactGroupIdentifier"));
            contactDataCommand.Acquaintance = GetAttributeValue(element, "acquaintance");
            contactDataCommand.PersonalHomePage = BuildHomePage(element);
            contactDataCommand.LendingLimit = int.Parse(GetAttributeValue(element, "lendingLimit"));
            contactDataCommand.PaymentTermIdentifier = int.Parse(GetAttributeValue(element, "paymentTermIdentifier"));

            return contactDataCommand;
        }

        private IAddressCommand BuildAddressCommand(XmlElement element)
        {
            NullGuard.NotNull(element, nameof(element));

            string addressLine1 = GetAttributeValue(element, "addressLine1");
            string addressLine2 = GetAttributeValue(element, "addressLine2");
            string postalCodeAndCity = GetAttributeValue(element, "postalCodeAndCity");
            if (string.IsNullOrWhiteSpace(addressLine1) && string.IsNullOrWhiteSpace(addressLine2) && string.IsNullOrWhiteSpace(postalCodeAndCity))
            {
                return null;
            }

            Match match = DanishPostalCodeAndCityRegex.Match(postalCodeAndCity);
            if (match.Success)
            {
                return new AddressCommand
                {
                    StreetLine1 = addressLine1,
                    StreetLine2 = addressLine2,
                    PostalCode = match.Groups[1].Value,
                    City = match.Groups[2].Value
                };
            }

            match = UnitedStatesPostalCodeAndCityRegex.Match(postalCodeAndCity);
            if (match.Success)
            {
                return new AddressCommand
                {
                    StreetLine1 = addressLine1,
                    StreetLine2 = addressLine2,
                    PostalCode = match.Groups[3].Value,
                    City = match.Groups[1].Value,
                    State = match.Groups[2].Value,
                    Country = "United States"
                };
            }

            match = GermanyPostalCodeAndCityRegex.Match(postalCodeAndCity);
            if (match.Success)
            {
                return new AddressCommand
                {
                    StreetLine1 = addressLine1,
                    StreetLine2 = addressLine2,
                    PostalCode = match.Groups[1].Value,
                    City = match.Groups[2].Value,
                    Country = "Germany"
                };
            }

            throw new NotSupportedException($"The value does not match any known postal code and city format: {postalCodeAndCity}");
        }

        private string BuildMailAddress(XmlElement element)
        {
            NullGuard.NotNull(element, nameof(element));

            string mailAddress = GetAttributeValue(element, "mailAddress");
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                return null;
            }

            return mailAddress.Trim().ToLower();
        }

        private string BuildHomePage(XmlElement element)
        {
            NullGuard.NotNull(element, nameof(element));

            string homePage = GetAttributeValue(element, "homePage");
            if (string.IsNullOrWhiteSpace(homePage))
            {
                return null;
            }

            string result = homePage.Trim().ToLower();

            return result.StartsWith("http://") || result.StartsWith("https://") ? result : $"http://{result}";
        }

        private string BuildPhoneNumber(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            Match match = DanishPhoneNumberRegex.Match(value);
            if (match.Success)
            {
                return $"+45 {match.Groups[2].Value} {match.Groups[3].Value} {match.Groups[4].Value} {match.Groups[5].Value}";
            }

            match = GermanyPhoneNumberRegex.Match(value);
            if (match.Success)
            {
                return $"+49 {match.Groups[1].Value} {match.Groups[2].Value}";
            }

            throw new NotSupportedException($"The value does not match any known phone number format: {value}");
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

            await using FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(fileStream, encoding);
            int lineNumber = 0;
            while (streamReader.EndOfStream == false)
            {
                await lineHandler(++lineNumber, streamReader.ReadLine());
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