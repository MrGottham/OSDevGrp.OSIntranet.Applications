using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
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
        private readonly IConverter _contactViewModelConverter = new ContactViewModelConverter();

        #endregion

        #region Constructor

        public ContactController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> Countries()
        {
            IEnumerable<ICountry> countries = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(new EmptyQuery());

            IEnumerable<CountryViewModel> countryViewModels = countries.AsParallel()
                .Select(country => _contactViewModelConverter.Convert<ICountry, CountryViewModel>(country))
                .OrderBy(countryViewModel => countryViewModel.Name)
                .ToList();

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

            IGetCountryQuery query = new GetCountryQuery
            {
                CountryCode = code
            };
            ICountry country = await _queryBus.QueryAsync<IGetCountryQuery, ICountry>(query);

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

        #endregion
    }
}
