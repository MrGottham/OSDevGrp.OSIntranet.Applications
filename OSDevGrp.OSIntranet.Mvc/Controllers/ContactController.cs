using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Attributes;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = Policies.ContactPolicy)]
    public class ContactController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IClaimResolver _claimResolver;
        private readonly ITokenHelperFactory _tokenHelperFactory;
        private readonly IConverter _contactViewModelConverter = new ContactViewModelConverter();
        private readonly IConverter _accountingViewModelConverter = new AccountingViewModelConverter();

        #endregion

        #region Constructor

        public ContactController(ICommandBus commandBus, IQueryBus queryBus, IClaimResolver claimResolver, ITokenHelperFactory tokenHelperFactory)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(tokenHelperFactory, nameof(tokenHelperFactory));

            _commandBus = commandBus;
            _queryBus = queryBus;
            _claimResolver = claimResolver;
            _tokenHelperFactory = tokenHelperFactory;
        }

        #endregion

        #region Methods

        [HttpGet]
        [AcquireToken(TokenType.MicrosoftGraphToken)]
        public async Task<IActionResult> Contacts(string filter = null, string externalIdentifier = null)
        {
            string defaultCountryCode = _claimResolver.GetCountryCode();

            IEnumerable<CountryViewModel> countryViewModels = await GetCountryViewModels();

            ContactOptionsViewModel contactOptionsViewModel = new ContactOptionsViewModel
            {
                Filter = string.IsNullOrWhiteSpace(filter) ? null : filter,
                ExternalIdentifier = string.IsNullOrWhiteSpace(externalIdentifier) ? null : externalIdentifier,
                DefaultCountryCode = defaultCountryCode,
                Countries = countryViewModels?.ToList() ?? new List<CountryViewModel>(0)
            };

            return View("Contacts", contactOptionsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> StartLoadingContacts(string filter = null, string externalIdentifier = null)
        {
            if (await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext) == null)
            {
                return Unauthorized();
            }

            ContactOptionsViewModel contactOptionsViewModel = new ContactOptionsViewModel
            {
                Filter = string.IsNullOrWhiteSpace(filter) ? null : filter,
                ExternalIdentifier = string.IsNullOrWhiteSpace(externalIdentifier) ? null : externalIdentifier
            };

            return PartialView("_LoadingContactsPartial", contactOptionsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoadContacts(string filter = null, string externalIdentifier = null)
        {
            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            IEnumerable<IContact> contacts;
            if (string.IsNullOrWhiteSpace(filter))
            {
                IGetContactCollectionQuery query = CreateContactQueryBase<GetContactCollectionQuery>(token);
                contacts = await _queryBus.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(query);
            }
            else
            {
                IGetMatchingContactCollectionQuery query = CreateContactQueryBase<GetMatchingContactCollectionQuery>(token);
                query.SearchFor = filter;
                query.SearchWithinName = true;
                query.SearchWithinMailAddress = true;
                query.SearchWithinHomePhone = true;
                query.SearchWithinMobilePhone = true;
                contacts = await _queryBus.QueryAsync<IGetMatchingContactCollectionQuery, IEnumerable<IContact>>(query);
            }

            IEnumerable<ContactInfoViewModel> contactInfoViewModels = contacts.AsParallel()
                .Select(_contactViewModelConverter.Convert<IContact, ContactInfoViewModel>)
                .OrderBy(contactInfoViewModel => contactInfoViewModel.DisplayName)
                .ToList();

            if (string.IsNullOrWhiteSpace(externalIdentifier) == false)
            {
                ViewData.Add("ExternalIdentifier", externalIdentifier);
            }

            return PartialView("_ContactCollectionPartial", contactInfoViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> StartLoadingContact(string externalIdentifier, string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(externalIdentifier, nameof(externalIdentifier))
                .NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            ContactIdentificationViewModel contactIdentificationViewModel = new ContactIdentificationViewModel
            {
                ExternalIdentifier = externalIdentifier,
                ContactType = ContactType.Unknown
            };

            ViewData.Add("CountryCode", countryCode);

            return PartialView("_LoadingContactPartial", contactIdentificationViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoadContact(string externalIdentifier, string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(externalIdentifier, nameof(externalIdentifier))
                .NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            IGetContactQuery getContactQuery = CreateContactQueryBase<GetContactQuery>(token);
            getContactQuery.ExternalIdentifier = externalIdentifier;

            List<ContactGroupViewModel> contactGroupViewModelCollection = (await GetContactGroupViewModels()).ToList();
            List<PaymentTermViewModel> paymentTermViewModelCollection = (await GetPaymentTermViewModels()).ToList();
            List<CountryViewModel> countryViewModelCollection = (await GetCountryViewModels()).ToList();
            ICountry country = await GetCountry(countryCode);
            IContact contact = await _queryBus.QueryAsync<IGetContactQuery, IContact>(getContactQuery);
            if (country == null || contact == null)
            {
                return BadRequest();
            }

            ContactViewModel contactViewModel = _contactViewModelConverter.Convert<IContact, ContactViewModel>(contact);
            contactViewModel.Country = _contactViewModelConverter.Convert<ICountry, CountryViewModel>(country);
            contactViewModel.Countries = countryViewModelCollection;
            contactViewModel.ContactGroups = contactGroupViewModelCollection;
            contactViewModel.PaymentTerms = paymentTermViewModelCollection;

            return PartialView("_ContactPartial", contactViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> StartCreatingContact(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            ContactOptionsViewModel contactOptionsViewModel = new ContactOptionsViewModel
            {
                DefaultCountryCode = countryCode
            };

            return PartialView("_CreatingContactPartial", contactOptionsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateContact(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            List<ContactGroupViewModel> contactGroupViewModelCollection = (await GetContactGroupViewModels()).ToList();
            List<PaymentTermViewModel> paymentTermViewModelCollection = (await GetPaymentTermViewModels()).ToList();
            List<CountryViewModel> countryViewModelCollection = (await GetCountryViewModels()).ToList();
            ICountry country = await GetCountry(countryCode);
            if (country == null)
            {
                return BadRequest();
            }

            ContactViewModel contactViewModel = new ContactViewModel
            {
                ContactType = ContactType.Person,
                HomePhone = country.PhonePrefix,
                MobilePhone = country.PhonePrefix,
                Address = new AddressViewModel
                {
                    Country = country.DefaultForPrincipal ? null : country.UniversalName
                },
                ContactGroup = contactGroupViewModelCollection.FirstOrDefault() ?? new ContactGroupViewModel(),
                PaymentTerm = paymentTermViewModelCollection.FirstOrDefault() ?? new PaymentTermViewModel(),
                Country = _contactViewModelConverter.Convert<ICountry, CountryViewModel>(country),
                Countries = countryViewModelCollection,
                ContactGroups = contactGroupViewModelCollection,
                LendingLimit = 14,
                PaymentTerms = paymentTermViewModelCollection,
                EditMode = EditMode.Create
            };

            return PartialView("_ContactPartial", contactViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> StartAddingAssociatedCompany(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            ContactOptionsViewModel contactOptionsViewModel = new ContactOptionsViewModel
            {
                DefaultCountryCode = countryCode
            };

            return PartialView("_AddingAssociatedCompanyPartial", contactOptionsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddAssociatedCompany(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            ICountry country = await GetCountry(countryCode);
            if (country == null)
            {
                return BadRequest();
            }

            CompanyViewModel companyViewModel = new CompanyViewModel
            {
                Address = new AddressViewModel
                {
                    Country = country.DefaultForPrincipal ? null : country.UniversalName
                },
                PrimaryPhone = country.PhonePrefix,
                SecondaryPhone = country.PhonePrefix
            };

            ViewData.TemplateInfo.HtmlFieldPrefix = "Company";

            return PartialView("_EditCompanyPartial", companyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact(ContactViewModel contactViewModel)
        {
            NullGuard.NotNull(contactViewModel, nameof(contactViewModel));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid == false)
            {
                return RedirectToAction("Contacts", "Contact");
            }

            ICreateContactCommand createContactCommand = _contactViewModelConverter.Convert<ContactViewModel, CreateContactCommand>(contactViewModel);
            createContactCommand.TokenType = token.TokenType;
            createContactCommand.AccessToken = token.AccessToken;
            createContactCommand.RefreshToken = token.RefreshToken;
            createContactCommand.Expires = token.Expires;

            await _commandBus.PublishAsync(createContactCommand);

            return RedirectToAction("Contacts", "Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContact(ContactViewModel contactViewModel)
        {
            NullGuard.NotNull(contactViewModel, nameof(contactViewModel));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid == false)
            {
                return RedirectToAction("Contacts", "Contact", new {contactViewModel.ExternalIdentifier});
            }

            IUpdateContactCommand updateContactCommand = _contactViewModelConverter.Convert<ContactViewModel, UpdateContactCommand>(contactViewModel);
            updateContactCommand.TokenType = token.TokenType;
            updateContactCommand.AccessToken = token.AccessToken;
            updateContactCommand.RefreshToken = token.RefreshToken;
            updateContactCommand.Expires = token.Expires;

            await _commandBus.PublishAsync(updateContactCommand);

            return RedirectToAction("Contacts", "Contact", new {contactViewModel.ExternalIdentifier});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(string externalIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalIdentifier, nameof(externalIdentifier));

            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            IDeleteContactCommand command = new DeleteContactCommand
            {
                ExternalIdentifier = externalIdentifier,
                TokenType = token.TokenType,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Expires = token.Expires
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Contacts", "Contact");
        }

        [HttpGet("/api/countries/action/export/csv")]
        public async Task<IActionResult> ExportContacts()
        {
            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            IExportContactCollectionQuery query = new ExportContactCollectionQuery
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Expires = token.Expires
            };
            byte[] fileContent = await _queryBus.QueryAsync<IExportContactCollectionQuery, byte[]>(query);

            return File(fileContent, "application/csv", $"{DateTime.Today:yyyyMMdd} - Contacts.csv");
        }

        [HttpGet]
        public async Task<IActionResult> ContactGroups()
        {
            IEnumerable<ContactGroupViewModel> contactGroupViewModels = await GetContactGroupViewModels();

            return View("ContactGroups", contactGroupViewModels);
        }

        [HttpGet]
        public IActionResult CreateContactGroup()
        {
            ContactGroupViewModel contactGroupViewModel = new ContactGroupViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreateContactGroup", contactGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContactGroup(ContactGroupViewModel contactGroupViewModel)
        {
            NullGuard.NotNull(contactGroupViewModel, nameof(contactGroupViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreateContactGroup", contactGroupViewModel);
            }

            ICreateContactGroupCommand command = _contactViewModelConverter.Convert<ContactGroupViewModel, CreateContactGroupCommand>(contactGroupViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("ContactGroups", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateContactGroup(int number)
        {
            IGetContactGroupQuery query = new GetContactGroupQuery
            {
                Number = number
            };
            IContactGroup contactGroup = await _queryBus.QueryAsync<IGetContactGroupQuery, IContactGroup>(query);

            ContactGroupViewModel contactGroupViewModel = _contactViewModelConverter.Convert<IContactGroup, ContactGroupViewModel>(contactGroup);
            contactGroupViewModel.EditMode = EditMode.Edit;

            return View("UpdateContactGroup", contactGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContactGroup(ContactGroupViewModel contactGroupViewModel)
        {
            NullGuard.NotNull(contactGroupViewModel, nameof(contactGroupViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdateContactGroup", contactGroupViewModel);
            }

            IUpdateContactGroupCommand command = _contactViewModelConverter.Convert<ContactGroupViewModel, UpdateContactGroupCommand>(contactGroupViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("ContactGroups", "Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContactGroup(int number)
        {
            IDeleteContactGroupCommand command = new DeleteContactGroupCommand
            {
                Number = number
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("ContactGroups", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> Countries()
        {
            IEnumerable<CountryViewModel> countryViewModels = await GetCountryViewModels();

            return View("Countries", countryViewModels);
        }

        [HttpGet]
        public IActionResult CreateCountry()
        {
            CountryViewModel countryViewModel = new CountryViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreateCountry", countryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry(CountryViewModel countryViewModel)
        {
            NullGuard.NotNull(countryViewModel, nameof(countryViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreateCountry", countryViewModel);
            }

            ICreateCountryCommand command = _contactViewModelConverter.Convert<CountryViewModel, CreateCountryCommand>(countryViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Countries", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCountry(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            ICountry country = await GetCountry(code);

            CountryViewModel countryViewModel = _contactViewModelConverter.Convert<ICountry, CountryViewModel>(country);
            countryViewModel.EditMode = EditMode.Edit;

            return View("UpdateCountry", countryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCountry(CountryViewModel countryViewModel)
        {
            NullGuard.NotNull(countryViewModel, nameof(countryViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdateCountry", countryViewModel);
            }

            IUpdateCountryCommand command = _contactViewModelConverter.Convert<CountryViewModel, UpdateCountryCommand>(countryViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Countries", "Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCountry(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            IDeleteCountryCommand command = new DeleteCountryCommand
            {
                CountryCode = code
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("Countries", "Contact");
        }

        [HttpGet]
        [Route("{controller}/{action}/")]
        public async Task<IActionResult> PostalCodes()
        {
            IEnumerable<CountryViewModel> countryViewModels = await GetCountryViewModels();

            return View("PostalCodes", countryViewModels);
        }

        [HttpGet]
        [Route("{controller}/{action}/{countryCode}/")]
        public async Task<IActionResult> PostalCodes(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            ICountry country = await GetCountry(countryCode);
            if (country == null)
            {
                return RedirectToAction("PostalCodes", "Contact");
            }

            IGetPostalCodeCollectionQuery query = new GetPostalCodeCollectionQuery
            {
                CountryCode = countryCode
            };
            IEnumerable<IPostalCode> postalCodes = await _queryBus.QueryAsync<IGetPostalCodeCollectionQuery, IEnumerable<IPostalCode>>(query);

            IEnumerable<PostalCodeViewModel> postalCodeViewModels = postalCodes.AsParallel()
                .Select(_contactViewModelConverter.Convert<IPostalCode, PostalCodeViewModel>)
                .OrderBy(postalCodeViewModel => postalCodeViewModel.City)
                .ToList();

            PartialViewResult result = PartialView("_PostalCodeTablePartial", postalCodeViewModels);
            result.ViewData.Add("CountryCode", countryCode);
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> CreatePostalCode(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            ICountry country = await GetCountry(countryCode);
            if (country == null)
            {
                return RedirectToAction("PostalCodes", "Contact");
            }

            PostalCodeViewModel postalCodeViewModel = new PostalCodeViewModel
            {
                Country = _contactViewModelConverter.Convert<ICountry, CountryViewModel>(country),
                EditMode = EditMode.Create
            };

            return View("CreatePostalCode", postalCodeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePostalCode(PostalCodeViewModel postalCodeViewModel)
        {
            NullGuard.NotNull(postalCodeViewModel, nameof(postalCodeViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreatePostalCode", postalCodeViewModel);
            }

            ICreatePostalCodeCommand command = _contactViewModelConverter.Convert<PostalCodeViewModel, CreatePostalCodeCommand>(postalCodeViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("PostalCodes", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePostalCode(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            IPostalCode postalCodeObj = await GetPostalCode(countryCode, postalCode);

            PostalCodeViewModel postalCodeViewModel = _contactViewModelConverter.Convert<IPostalCode, PostalCodeViewModel>(postalCodeObj);
            postalCodeViewModel.EditMode = EditMode.Edit;

            return View("UpdatePostalCode", postalCodeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePostalCode(PostalCodeViewModel postalCodeViewModel)
        {
            NullGuard.NotNull(postalCodeViewModel, nameof(postalCodeViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdatePostalCode", postalCodeViewModel);
            }

            IUpdatePostalCodeCommand command = _contactViewModelConverter.Convert<PostalCodeViewModel, UpdatePostalCodeCommand>(postalCodeViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("PostalCodes", "Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostalCode(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            IDeletePostalCodeCommand command = new DeletePostalCodeCommand
            {
                CountryCode = countryCode,
                PostalCode = postalCode
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("PostalCodes", "Contact");
        }

        [HttpGet("/api/countries/{countryCode}/postalcodes/{postalCode}")]
        public async Task<IActionResult> ResolvePostalCode(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            IPostalCode postalCodeObj = await GetPostalCode(countryCode, postalCode);

            return Ok(_contactViewModelConverter.Convert<IPostalCode, PostalCodeViewModel>(postalCodeObj));
        }

        private T CreateContactQueryBase<T>(IRefreshableToken refreshableToken) where T : class, IContactQuery, new()
        {
            NullGuard.NotNull(refreshableToken, nameof(refreshableToken));

            return new T
            {
                TokenType = refreshableToken.TokenType,
                AccessToken = refreshableToken.AccessToken,
                RefreshToken = refreshableToken.RefreshToken,
                Expires = refreshableToken.Expires
            };
        }

        private async Task<IEnumerable<ContactGroupViewModel>> GetContactGroupViewModels()
        {
            IEnumerable<IContactGroup> contactGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(new EmptyQuery());

            return contactGroups.AsParallel()
                .Select(_contactViewModelConverter.Convert<IContactGroup, ContactGroupViewModel>)
                .OrderBy(contactGroupViewModel => contactGroupViewModel.Number)
                .ToList();
        }

        private async Task<IEnumerable<CountryViewModel>> GetCountryViewModels()
        {
            IEnumerable<ICountry> countries = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(new EmptyQuery());

            return countries.AsParallel()
                .Select(_contactViewModelConverter.Convert<ICountry, CountryViewModel>)
                .OrderBy(countryViewModel => countryViewModel.Name)
                .ToList();
        }

        private async Task<ICountry> GetCountry(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            IGetCountryQuery query = new GetCountryQuery
            {
                CountryCode = countryCode
            };
            return await _queryBus.QueryAsync<IGetCountryQuery, ICountry>(query);
        }

        private async Task<IPostalCode> GetPostalCode(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            IGetPostalCodeQuery query = new GetPostalCodeQuery
            {
                CountryCode = countryCode,
                PostalCode = postalCode
            };
            return await _queryBus.QueryAsync<IGetPostalCodeQuery, IPostalCode>(query);
        }

        private async Task<IEnumerable<PaymentTermViewModel>> GetPaymentTermViewModels()
        {
            IEnumerable<IPaymentTerm> paymentTerms = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(new EmptyQuery());

            return paymentTerms.AsParallel()
                .Select(_accountingViewModelConverter.Convert<IPaymentTerm, PaymentTermViewModel>)
                .OrderBy(paymentTermViewModel => paymentTermViewModel.Number)
                .ToList();
        }

        #endregion
    }
}