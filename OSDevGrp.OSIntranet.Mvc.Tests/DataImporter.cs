using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries;
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
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
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
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Core.Resolvers;
using OSDevGrp.OSIntranet.Domain.Accounting;
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

        private static readonly Regex PersonNameRegex = new Regex(@"^([A-Z�����][A-Z�����a-z�����\-]+\s)?([A-Z�����][A-Z�����a-z�����.\s]+\s)?([A-Z�����][A-Z�����a-z�����\-]+)$", RegexOptions.Compiled);
        private static readonly Regex DanishPostalCodeAndCityRegex = new Regex(@"^([0-9]{4})\s+([A-Z���][A-Z���a-z���\s]*)$", RegexOptions.Compiled);
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
            IEventPublisher eventPublisher = CreateEventPublisher();

            _validator = CreateValidator();

            IClaimResolver claimResolver = new ClaimResolver(principalResolver);
            ICountryHelper countryHelper = new CountryHelper(claimResolver);
            IAccountingHelper accountingHelper = new AccountingHelper(claimResolver);
            IPostingWarningCalculator postingWarningCalculator = new PostingWarningCalculator();

            ICommonRepository commonRepository = new CommonRepository(configuration, principalResolver, loggerFactory);

            _microsoftGraphRepository = new MicrosoftGraphRepository(configuration, principalResolver, loggerFactory);
            _contactRepository = new ContactRepository(configuration, principalResolver, loggerFactory);
            _accountingRepository = new AccountingRepository(configuration, principalResolver, loggerFactory, eventPublisher);

            ICommandHandler<IRefreshTokenForMicrosoftGraphCommand, IRefreshableToken> refreshTokenForMicrosoftGraphCommandHandler = new RefreshTokenForMicrosoftGraphCommandHandler(_microsoftGraphRepository);
            ICommandHandler<ICreateLetterHeadCommand> createLetterHeadCommandHandler = new CreateLetterHeadCommandHandler(_validator, commonRepository);
            ICommandHandler<ICreateContactCommand> createContactCommandHandler = new CreateContactCommandHandler(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);
            ICommandHandler<IUpdateContactCommand> updateContactCommandHandler = new UpdateContactCommandHandler(_validator, _microsoftGraphRepository, _contactRepository, _accountingRepository);
            ICommandHandler<ICreateContactGroupCommand> createContactGroupCommandHandler = new CreateContactGroupCommandHandler(_validator, _contactRepository);
            ICommandHandler<ICreatePostalCodeCommand> createPostalCodeCommandHandler = new CreatePostalCodeCommandHandler(_validator, _contactRepository);
            ICommandHandler<ICreateAccountingCommand> createAccountingCommandHandler = new CreateAccountingCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<IUpdateAccountingCommand> updateAccountingCommandHandler = new UpdateAccountingCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<ICreateAccountCommand> createAccountCommandHandler = new CreateAccountCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<IUpdateAccountCommand> updateAccountCommandHandler = new UpdateAccountCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<ICreateBudgetAccountCommand> createBudgetAccountCommandHandler = new CreateBudgetAccountCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<IUpdateBudgetAccountCommand> updateBudgetAccountCommandHandler = new UpdateBudgetAccountCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<ICreateContactAccountCommand> createContactAccountCommandHandler = new CreateContactAccountCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<IUpdateContactAccountCommand> updateContactAccountCommandHandler = new UpdateContactAccountCommandHandler(_validator, _accountingRepository, commonRepository);
            ICommandHandler<IApplyPostingJournalCommand, IPostingJournalResult> applyPostingJournalCommandHandler = new ApplyPostingJournalCommandHandler(_validator, _accountingRepository, commonRepository, postingWarningCalculator);
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
                createAccountCommandHandler,
                updateAccountCommandHandler,
                createBudgetAccountCommandHandler,
                updateBudgetAccountCommandHandler,
                createContactAccountCommandHandler,
                updateContactAccountCommandHandler,
                applyPostingJournalCommandHandler,
                createPaymentTermCommandHandler
            });

            IQueryHandler<IGetMatchingContactCollectionQuery, IEnumerable<IContact>> getMatchingContactCollectionQueryHandler = new GetMatchingContactCollectionQueryHandler(_validator, _microsoftGraphRepository, _contactRepository);
            IQueryHandler<EmptyQuery, IEnumerable<ICountry>> getCountryCollectionQueryHandler = new GetCountryCollectionQueryHandler(_contactRepository, countryHelper);
            IQueryHandler<IGetPostalCodeQuery, IPostalCode> getPostalCodeQueryHandler = new GetPostalCodeQueryHandler(_validator, _contactRepository, countryHelper);
            IQueryHandler<EmptyQuery, IEnumerable<IAccounting>> getAccountingCollectionQueryHandler = new GetAccountingCollectionQueryHandler(_accountingRepository, accountingHelper);
            IQueryHandler<IGetAccountCollectionQuery, IAccountCollection> getAccountCollectionQueryHandler = new GetAccountCollectionQueryHandler(_validator, _accountingRepository);
            IQueryHandler<IGetAccountingQuery, IAccounting> getAccountingQueryHandler = new GetAccountingQueryHandler(_validator, _accountingRepository, accountingHelper);
            IQueryHandler<IGetBudgetAccountCollectionQuery, IBudgetAccountCollection> getBudgetAccountCollectionQueryHandler = new GetBudgetAccountCollectionQueryHandler(_validator, _accountingRepository);
            IQueryHandler<IGetContactAccountCollectionQuery, IContactAccountCollection> getContactAccountCollectionQueryHandler = new GetContactAccountCollectionQueryHandler(_validator, _accountingRepository);
            _queryBus = new QueryBus(new IQueryHandler[]
            {
                getMatchingContactCollectionQueryHandler,
                getCountryCollectionQueryHandler,
                getPostalCodeQueryHandler,
                getAccountingCollectionQueryHandler,
                getAccountingQueryHandler,
                getAccountCollectionQueryHandler,
                getBudgetAccountCollectionQueryHandler,
                getContactAccountCollectionQueryHandler
            });
        }

        [Test]
        [Category("DataImport")]
        [TestCase("LetterHeads.csv")]
        [Ignore("Test which imports data and should only be run once")]
        public async Task Import_LetterHeads_FromFile(string fileName)
        {
            await ImportFromFile(fileName, Encoding.UTF8, async (lineNumber, line) =>
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

            XmlNodeList accountingNodeList = accountingDocument.DocumentElement?.SelectNodes("Accounting");
            if (accountingNodeList == null)
            {
                return;
            }
            
            DateTime infoFromDate = DateTime.Today;

            IDictionary<int, IAccounting> accountingDictionary = (await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(new EmptyQuery())).ToDictionary(accounting => accounting.Number, accounting => accounting);

            foreach (XmlElement accountingElement in accountingNodeList.OfType<XmlElement>())
            {
                IAccounting accounting = await HandleAccountingElementAsync(accountingElement, accountingDictionary);
                if (accounting == null)
                {
                    continue;
                }

                IDictionary<string, IAccount> accountDictionary = (await _queryBus.QueryAsync<IGetAccountCollectionQuery, IAccountCollection>(new GetAccountCollectionQuery {AccountingNumber = accounting.Number, StatusDate = DateTime.Today})).ToDictionary(m => m.AccountNumber, m => m);
                IDictionary<string, IBudgetAccount> budgetAccountDictionary = (await _queryBus.QueryAsync<IGetBudgetAccountCollectionQuery, IBudgetAccountCollection>(new GetBudgetAccountCollectionQuery {AccountingNumber = accounting.Number, StatusDate = DateTime.Today})).ToDictionary(m => m.AccountNumber, m => m);
                IDictionary<string, IContactAccount> contactAccountDictionary = (await _queryBus.QueryAsync<IGetContactAccountCollectionQuery, IContactAccountCollection>(new GetContactAccountCollectionQuery {AccountingNumber = accounting.Number, StatusDate = DateTime.Today})).ToDictionary(m => m.AccountNumber, m => m);

                await HandleAccountElementCollectionAsync(accounting.Number, accountingElement.SelectNodes("Account"), accountDictionary, infoFromDate);
                await HandleBudgetAccountElementCollectionAsync(accounting.Number, accountingElement.SelectNodes("BudgetAccount"), budgetAccountDictionary, infoFromDate);
                IReadOnlyDictionary<int, string> contactAccountConvertingMap = await HandleContactAccountElementCollectionAsync(accounting.Number, accountingElement.SelectNodes("ContactAccount"), contactAccountDictionary);

                XmlNodeList postingLineElementCollection = accountingElement.SelectNodes("PostingLine");

                DateTime? latestPostingDate = (await accounting.GetPostingLinesAsync(DateTime.Today))
                    .Ordered()
                    .FirstOrDefault()?
                    .PostingDate.Date;

                DateTime fromPostingDate;
                if (latestPostingDate != null)
                {
                    fromPostingDate = latestPostingDate.Value.AddDays(1).Date;
                }
                else
                {
                    fromPostingDate = postingLineElementCollection?.OfType<XmlElement>()
                                          .Select(GetPostingDate)
                                          .OrderBy(postingDate => postingDate.Date)
                                          .FirstOrDefault().Date ??
                                      DateTime.MinValue;
                }

                DateTime toPostingDate = new DateTime(fromPostingDate.Year, 12, 31).Date;
                if (toPostingDate.Year == fromPostingDate.Year)
                {
                    toPostingDate = toPostingDate.AddYears(1).Date;
                }
                if (toPostingDate.Date >= DateTime.Today)
                {
                    toPostingDate = toPostingDate.AddMonths(-1).Date;
                }

                await HandlePostingLineElementCollectionAsync(accounting.Number, postingLineElementCollection, contactAccountConvertingMap, fromPostingDate, toPostingDate);
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

        private async Task<IAccounting> HandleAccountingElementAsync(XmlElement accountingElement, IDictionary<int, IAccounting> accountingDictionary)
        {
            NullGuard.NotNull(accountingElement, nameof(accountingElement))
                .NotNull(accountingElement, nameof(accountingDictionary));

            if (int.TryParse(GetAttributeValue(accountingElement, "number"), out int accountingNumber) == false)
            {
                return null;
            }

            string accountingName = GetAttributeValue(accountingElement, "name");
            if (string.IsNullOrWhiteSpace(accountingName))
            {
                return null;
            }

            if (int.TryParse(GetAttributeValue(accountingElement, "letterHeadNumber"), out int letterHeadNumber) == false)
            {
                return null;
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

                return await _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(new GetAccountingQuery {AccountingNumber = accountingNumber, StatusDate = DateTime.Today});
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

            return await _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(new GetAccountingQuery {AccountingNumber = accountingNumber, StatusDate = DateTime.Today});
        }

        private async Task HandleAccountElementCollectionAsync(int accountingNumber, XmlNodeList accountElementCollection, IDictionary<string, IAccount> accountDictionary, DateTime infoFromDate)
        {
            NullGuard.NotNull(accountElementCollection, nameof(accountElementCollection))
                .NotNull(accountDictionary, nameof(accountDictionary));

            foreach (XmlElement accountElement in accountElementCollection.OfType<XmlElement>())
            {
                string accountNumber = GetAttributeValue(accountElement, "accountNumber")?.ToUpper();
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    continue;
                }

                string accountName = GetAttributeValue(accountElement, "accountName");
                if (string.IsNullOrWhiteSpace(accountName))
                {
                    continue;
                }

                if (int.TryParse(GetAttributeValue(accountElement, "accountGroup"), out int accountGroupNumber) == false)
                {
                    continue;
                }

                IList<ICreditInfoCommand> creditInfoCommandCollection = new List<ICreditInfoCommand>();
                XmlNodeList creditInfoNodeList = accountElement.SelectNodes("CreditInfo");
                if (creditInfoNodeList != null)
                {
                    foreach (XmlElement creditInfoElement in creditInfoNodeList.OfType<XmlElement>())
                    {
                        if (short.TryParse(GetAttributeValue(creditInfoElement, "year"), out short year) == false)
                        {
                            continue;
                        }

                        if (short.TryParse(GetAttributeValue(creditInfoElement, "month"), out short month) == false)
                        {
                            continue;
                        }

                        if (year < (short) infoFromDate.Year || year == (short) infoFromDate.Year && month < (short) infoFromDate.Month)
                        {
                            continue;
                        }

                        if (decimal.TryParse(GetAttributeValue(creditInfoElement, "credit"), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal credit) == false)
                        {
                            continue;
                        }

                        ICreditInfoCommand creditInfoCommand = new CreditInfoCommand
                        {
                            Year = year,
                            Month = month,
                            Credit = credit
                        };
                        creditInfoCommandCollection.Add(creditInfoCommand);
                    }
                }

                if (accountDictionary.ContainsKey(accountNumber))
                {
                    IUpdateAccountCommand updateAccountCommand = new UpdateAccountCommand
                    {
                        AccountingNumber = accountingNumber,
                        AccountNumber = accountNumber,
                        AccountName = accountName,
                        Description = GetAttributeValue(accountElement, "description") ?? accountDictionary[accountNumber].Description,
                        Note = GetAttributeValue(accountElement, "note") ?? accountDictionary[accountNumber].Note,
                        AccountGroupNumber = accountGroupNumber,
                        CreditInfoCollection = creditInfoCommandCollection
                    };
                    await _commandBus.PublishAsync(updateAccountCommand);

                    continue;
                }

                ICreateAccountCommand createAccountCommand = new CreateAccountCommand
                {
                    AccountingNumber = accountingNumber,
                    AccountNumber = accountNumber,
                    AccountName = accountName,
                    Description = GetAttributeValue(accountElement, "description"),
                    Note = GetAttributeValue(accountElement, "note"),
                    AccountGroupNumber = accountGroupNumber,
                    CreditInfoCollection = creditInfoCommandCollection
                };
                await _commandBus.PublishAsync(createAccountCommand);
            }
        }

        private async Task HandleBudgetAccountElementCollectionAsync(int accountingNumber, XmlNodeList budgetAccountElementCollection, IDictionary<string, IBudgetAccount> budgetAccountDictionary, DateTime infoFromDate)
        {
            NullGuard.NotNull(budgetAccountElementCollection, nameof(budgetAccountElementCollection))
                .NotNull(budgetAccountDictionary, nameof(budgetAccountDictionary));

            foreach (XmlElement budgetAccountElement in budgetAccountElementCollection.OfType<XmlElement>())
            {
                string accountNumber = GetAttributeValue(budgetAccountElement, "accountNumber")?.ToUpper();
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    continue;
                }

                string accountName = GetAttributeValue(budgetAccountElement, "accountName");
                if (string.IsNullOrWhiteSpace(accountName))
                {
                    continue;
                }

                if (int.TryParse(GetAttributeValue(budgetAccountElement, "budgetAccountGroup"), out int budgetAccountGroupNumber) == false)
                {
                    continue;
                }

                IList<IBudgetInfoCommand> budgetInfoCommandCollection = new List<IBudgetInfoCommand>();
                XmlNodeList budgetInfoNodeList = budgetAccountElement.SelectNodes("BudgetInfo");
                if (budgetInfoNodeList != null)
                {
                    foreach (XmlElement budgetInfoElement in budgetInfoNodeList.OfType<XmlElement>())
                    {
                        if (short.TryParse(GetAttributeValue(budgetInfoElement, "year"), out short year) == false)
                        {
                            continue;
                        }

                        if (short.TryParse(GetAttributeValue(budgetInfoElement, "month"), out short month) == false)
                        {
                            continue;
                        }

                        if (year < (short) infoFromDate.Year || year == (short) infoFromDate.Year && month < (short) infoFromDate.Month)
                        {
                            continue;
                        }

                        if (decimal.TryParse(GetAttributeValue(budgetInfoElement, "income"), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal income) == false)
                        {
                            continue;
                        }

                        if (decimal.TryParse(GetAttributeValue(budgetInfoElement, "expenses"), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal expenses) == false)
                        {
                            continue;
                        }

                        IBudgetInfoCommand budgetInfoCommand = new BudgetInfoCommand
                        {
                            Year = year,
                            Month = month,
                            Income = income,
                            Expenses = expenses
                        };
                        budgetInfoCommandCollection.Add(budgetInfoCommand);
                    }
                }

                if (budgetAccountDictionary.ContainsKey(accountNumber))
                {
                    IUpdateBudgetAccountCommand updateBudgetAccountCommand = new UpdateBudgetAccountCommand
                    {
                        AccountingNumber = accountingNumber,
                        AccountNumber = accountNumber,
                        AccountName = accountName,
                        Description = GetAttributeValue(budgetAccountElement, "description") ?? budgetAccountDictionary[accountNumber].Description,
                        Note = GetAttributeValue(budgetAccountElement, "note") ?? budgetAccountDictionary[accountNumber].Note,
                        BudgetAccountGroupNumber = budgetAccountGroupNumber,
                        BudgetInfoCollection = budgetInfoCommandCollection
                    };
                    await _commandBus.PublishAsync(updateBudgetAccountCommand);

                    continue;
                }

                ICreateBudgetAccountCommand createBudgetAccountCommand = new CreateBudgetAccountCommand
                {
                    AccountingNumber = accountingNumber,
                    AccountNumber = accountNumber,
                    AccountName = accountName,
                    Description = GetAttributeValue(budgetAccountElement, "description"),
                    Note = GetAttributeValue(budgetAccountElement, "note"),
                    BudgetAccountGroupNumber = budgetAccountGroupNumber,
                    BudgetInfoCollection = budgetInfoCommandCollection
                };
                await _commandBus.PublishAsync(createBudgetAccountCommand);
            }
        }

        private async Task<IReadOnlyDictionary<int, string>> HandleContactAccountElementCollectionAsync(int accountingNumber, XmlNodeList contactAccountElementCollection, IDictionary<string, IContactAccount> contactAccountDictionary)
        {
            NullGuard.NotNull(contactAccountElementCollection, nameof(contactAccountElementCollection))
                .NotNull(contactAccountDictionary, nameof(contactAccountDictionary));

            IDictionary<int, string> contactAccountConvertingMap = new ConcurrentDictionary<int, string>();
            foreach (XmlElement contactAccountElement in contactAccountElementCollection.OfType<XmlElement>())
            {
                string accountNumber = GetAttributeValue(contactAccountElement, "primaryPhone")?.Replace(" ", string.Empty).ToUpper();
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    continue;
                }

                string accountName = GetAttributeValue(contactAccountElement, "accountName");
                if (string.IsNullOrWhiteSpace(accountName))
                {
                    continue;
                }

                if (int.TryParse(GetAttributeValue(contactAccountElement, "paymentTermNumber"), out int paymentTermNumber) == false)
                {
                    continue;
                }

                string primaryPhone = GetAttributeValue(contactAccountElement, "primaryPhone");
                if (string.IsNullOrWhiteSpace(primaryPhone) == false)
                {
                    primaryPhone = BuildPhoneNumber(primaryPhone);
                }

                string secondaryPhone = GetAttributeValue(contactAccountElement, "secondaryPhone");
                if (string.IsNullOrWhiteSpace(secondaryPhone) == false)
                {
                    secondaryPhone = BuildPhoneNumber(secondaryPhone);
                }

                if (contactAccountDictionary.ContainsKey(accountNumber))
                {
                    IUpdateContactAccountCommand updateContactAccountCommand = new UpdateContactAccountCommand
                    {
                        AccountingNumber = accountingNumber,
                        AccountNumber = accountNumber,
                        AccountName = accountName,
                        Description = GetAttributeValue(contactAccountElement, "description") ?? contactAccountDictionary[accountNumber].Description,
                        Note = GetAttributeValue(contactAccountElement, "note") ?? contactAccountDictionary[accountNumber].Note,
                        PrimaryPhone = primaryPhone ?? contactAccountDictionary[accountNumber].PrimaryPhone,
                        SecondaryPhone = secondaryPhone ?? contactAccountDictionary[accountNumber].SecondaryPhone,
                        MailAddress = BuildMailAddress(contactAccountElement) ?? contactAccountDictionary[accountNumber].MailAddress,
                        PaymentTermNumber = paymentTermNumber
                    };
                    await _commandBus.PublishAsync(updateContactAccountCommand);

                    contactAccountConvertingMap.Add(int.Parse(GetAttributeValue(contactAccountElement, "accountNumber")), accountNumber);

                    continue;
                }

                ICreateContactAccountCommand createContactAccountCommand = new CreateContactAccountCommand
                {
                    AccountingNumber = accountingNumber,
                    AccountNumber = accountNumber,
                    AccountName = accountName,
                    Description = GetAttributeValue(contactAccountElement, "description"),
                    Note = GetAttributeValue(contactAccountElement, "note"),
                    PrimaryPhone = primaryPhone,
                    SecondaryPhone = secondaryPhone,
                    MailAddress = BuildMailAddress(contactAccountElement),
                    PaymentTermNumber = paymentTermNumber
                };
                await _commandBus.PublishAsync(createContactAccountCommand);

                contactAccountConvertingMap.Add(int.Parse(GetAttributeValue(contactAccountElement, "accountNumber")), accountNumber);
            }

            return new ReadOnlyDictionary<int, string>(contactAccountConvertingMap);
        }

        private async Task HandlePostingLineElementCollectionAsync(int accountingNumber, XmlNodeList postingLineElementCollection, IReadOnlyDictionary<int, string> contactAccountConvertingMap, DateTime fromPostingDate, DateTime toPostingDate)
        {
            NullGuard.NotNull(postingLineElementCollection, nameof(postingLineElementCollection))
                .NotNull(contactAccountConvertingMap, nameof(contactAccountConvertingMap));

            IApplyPostingJournalCommand[] applyPostingJournalCommandCollection = postingLineElementCollection.OfType<XmlElement>()
                .Where(postingLineElement =>
                {
                    DateTime postingDate = GetPostingDate(postingLineElement);
                    return postingDate.Date >= fromPostingDate.Date && postingDate.Date <= toPostingDate.Date;
                })
                .Select(postingLineElement => BuildApplyPostingLineCommand(postingLineElement, contactAccountConvertingMap))
                .GroupBy(applyPostingLineCommand => applyPostingLineCommand.PostingDate.Year * 100 + applyPostingLineCommand.PostingDate.Month)
                .OrderBy(group => group.Key)
                .Select(group =>
                {
                    IApplyPostingJournalCommand applyPostingJournalCommand = new ApplyPostingJournalCommand
                    {
                        AccountingNumber = accountingNumber,
                        PostingLineCollection = group.OrderBy(m => m.PostingDate.Date)
                            .ThenBy(m => m.SortOrder)
                            .ToArray()
                    };
                    return applyPostingJournalCommand;
                })
                .ToArray();

            foreach (IApplyPostingJournalCommand applyPostingJournalCommand in applyPostingJournalCommandCollection)
            {
                await _commandBus.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(applyPostingJournalCommand);
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

        private IApplyPostingLineCommand BuildApplyPostingLineCommand(XmlElement element, IReadOnlyDictionary<int, string> contactAccountConvertingMap)
        {
            NullGuard.NotNull(element, nameof(element))
                .NotNull(contactAccountConvertingMap, nameof(contactAccountConvertingMap));

            return new ApplyPostingLineCommand
            {
                PostingDate = GetPostingDate(element),
                Reference = GetAttributeValue(element, "reference"),
                AccountNumber = GetAttributeValue(element, "accountNumber"),
                Details = GetAttributeValue(element, "details"),
                BudgetAccountNumber = GetAttributeValue(element, "budgetAccountNumber"),
                Debit = GetDecimalFromAttributeValue(element, "debit"),
                Credit = GetDecimalFromAttributeValue(element, "credit"),
                ContactAccountNumber = GetContactAccountNumber(element, contactAccountConvertingMap),
                SortOrder = int.Parse(GetAttributeValue(element, "sortOrder"), NumberStyles.Any, CultureInfo.InvariantCulture)
            };
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
            return new Validator(new IntegerValidator(), new DecimalValidator(), new StringValidator(), new DateTimeValidator(), new ObjectValidator(), new EnumerableValidator());
        }

        private IEventPublisher CreateEventPublisher()
        {
            return new EventPublisher();
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

        private DateTime GetPostingDate(XmlElement element)
        {
            NullGuard.NotNull(element, nameof(element));

            return DateTime.ParseExact(GetAttributeValue(element, "postingDate"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private string GetContactAccountNumber(XmlElement element, IReadOnlyDictionary<int, string> contactAccountConvertingMap)
        {
            NullGuard.NotNull(element, nameof(element))
                .NotNull(contactAccountConvertingMap, nameof(contactAccountConvertingMap));

            string attributeValue = GetAttributeValue(element, "contactAccountNumber");
            if (string.IsNullOrWhiteSpace(attributeValue))
            {
                return null;
            }

            int contactAccountNumber = int.Parse(attributeValue, NumberStyles.Any, CultureInfo.InvariantCulture);
            return contactAccountConvertingMap.ContainsKey(contactAccountNumber) ? contactAccountConvertingMap[contactAccountNumber] : null;
        }

        private decimal? GetDecimalFromAttributeValue(XmlElement element, string attributeName)
        {
            NullGuard.NotNull(element, nameof(element))
                .NotNullOrWhiteSpace(attributeName, nameof(attributeName));

            string attributeValue = GetAttributeValue(element, attributeName);
            if (string.IsNullOrWhiteSpace(attributeValue))
            {
                return null;
            }

            return decimal.Parse(attributeValue, NumberStyles.Any, CultureInfo.InvariantCulture);
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