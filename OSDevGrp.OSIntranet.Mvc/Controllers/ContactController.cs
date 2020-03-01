using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Attributes;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "Contacts")]
    public class ContactController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IClaimResolver _claimResolver;
        private readonly ITokenHelperFactory _tokenHelperFactory;
        private readonly IConverter _contactViewModelConverter = new ContactViewModelConverter();

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
        public IActionResult Contacts(string filter = null)
        {
            ContactOptionsViewModel contactOptionsViewModel = new ContactOptionsViewModel
            {
                Filter = string.IsNullOrWhiteSpace(filter) ? null : filter,
                DefaultCountryCode = _claimResolver.GetCountryCode()
            };

            return View("Contacts", contactOptionsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ContactGroups()
        {
            IEnumerable<IContactGroup> contactGroups =  await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(new EmptyQuery());

            IEnumerable<ContactGroupViewModel> contactGroupViewModels = contactGroups.AsParallel()
                .Select(contactGroup => _contactViewModelConverter.Convert<IContactGroup, ContactGroupViewModel>(contactGroup))
                .OrderBy(contactGroupViewModel => contactGroupViewModel.Number)
                .ToList();

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
                .Select(postalCode => _contactViewModelConverter.Convert<IPostalCode, PostalCodeViewModel>(postalCode))
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

            IGetPostalCodeQuery query = new GetPostalCodeQuery
            {
                CountryCode = countryCode,
                PostalCode = postalCode
            };
            IPostalCode postalCodeObj = await _queryBus.QueryAsync<IGetPostalCodeQuery, IPostalCode>(query);

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

        private async Task<IEnumerable<CountryViewModel>> GetCountryViewModels()
        {
            IEnumerable<ICountry> countries = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(new EmptyQuery());

            return countries.AsParallel()
                .Select(country => _contactViewModelConverter.Convert<ICountry, CountryViewModel>(country))
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

        #endregion
    }
}