using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = Policies.CommonDataPolicy)]
    public class CommonController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IConverter _commonViewModelConverter = new CommonViewModelConverter();

        #endregion

        #region Constructor

        public CommonController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> LetterHeads()
        {
            IEnumerable<ILetterHead> letterHeads = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(new EmptyQuery());

            IEnumerable<LetterHeadViewModel> letterHeadViewModels = letterHeads.AsParallel()
                .Select(_commonViewModelConverter.Convert<ILetterHead, LetterHeadViewModel>)
                .OrderBy(letterHeadViewModel => letterHeadViewModel.Number)
                .ToList();

            return View("LetterHeads", letterHeadViewModels);
        }

        [HttpGet]
        public IActionResult CreateLetterHead()
        {
            LetterHeadViewModel letterHeadViewModel = new LetterHeadViewModel
            {
                EditMode = EditMode.Create
            };

            return View("CreateLetterHead", letterHeadViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLetterHead(LetterHeadViewModel letterHeadViewModel)
        {
            NullGuard.NotNull(letterHeadViewModel, nameof(letterHeadViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreateLetterHead", letterHeadViewModel);
            }

            ICreateLetterHeadCommand command = _commonViewModelConverter.Convert<LetterHeadViewModel, CreateLetterHeadCommand>(letterHeadViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("LetterHeads", "Common");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLetterHead(int number)
        {
            IGetLetterHeadQuery query = new GetLetterHeadQuery
            {
                Number = number
            };
            ILetterHead letterHead = await _queryBus.QueryAsync<IGetLetterHeadQuery, ILetterHead>(query);

            LetterHeadViewModel letterHeadViewModel = _commonViewModelConverter.Convert<ILetterHead, LetterHeadViewModel>(letterHead);
            letterHeadViewModel.EditMode = EditMode.Edit;

            return View("UpdateLetterHead", letterHeadViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLetterHead(LetterHeadViewModel letterHeadViewModel)
        {
            NullGuard.NotNull(letterHeadViewModel, nameof(letterHeadViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdateLetterHead", letterHeadViewModel);
            }

            IUpdateLetterHeadCommand command = _commonViewModelConverter.Convert<LetterHeadViewModel, UpdateLetterHeadCommand>(letterHeadViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("LetterHeads", "Common");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLetterHead(int number)
        {
            IDeleteLetterHeadCommand command = new DeleteLetterHeadCommand
            {
                Number = number
            };
            await _commandBus.PublishAsync(command);

            return RedirectToAction("LetterHeads", "Common");
        }

        [HttpGet]
        public async Task<IActionResult> Nationalities()
        {
            IEnumerable<INationality> nationalities = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<INationality>>(CommonQueryFactory.BuildEmptyQuery());

            return GenericCategoryCollectionViewModel.Create("Nationaliteter", "Common", nameof(CreateNationality), nameof(UpdateNationality), (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("Common", nameof(DeleteNationality), urlHelper), nationalities ?? Array.Empty<INationality>(), _commonViewModelConverter)
                .AsView(this);
        }

        [HttpGet]
        public IActionResult CreateNationality()
        {
	        return GenericCategoryViewModel.Create("Opret nationalitet", "Common", nameof(CreateNationality), nameof(Nationalities))
		        .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNationality(GenericCategoryViewModel genericCategoryViewModel)
        {
	        if (genericCategoryViewModel == null)
	        {
		        return BadRequest();
	        }

	        if (ModelState.IsValid == false)
	        {
		        return BadRequest(ModelState);
	        }

	        await _commandBus.PublishAsync(CommonCommandFactory.BuildCreateNationalityCommand(genericCategoryViewModel.Number, genericCategoryViewModel.Name));

	        return RedirectToAction(nameof(Nationalities), "Common");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateNationality(int number)
        {
            INationality nationality = await _queryBus.QueryAsync<IGetNationalityQuery, INationality>(CommonQueryFactory.BuildGetNationalityQuery(number));
            if (nationality == null)
            {
                return BadRequest();
            }

            return GenericCategoryViewModel.Create("Redigér nationalitet", "Common", nameof(UpdateNationality), nameof(Nationalities), nationality)
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNationality(GenericCategoryViewModel genericCategoryViewModel)
        {
	        if (genericCategoryViewModel == null)
	        {
		        return BadRequest();
	        }

	        if (ModelState.IsValid == false)
	        {
		        return BadRequest(ModelState);
	        }

	        await _commandBus.PublishAsync(CommonCommandFactory.BuildUpdateNationalityCommand(genericCategoryViewModel.Number, genericCategoryViewModel.Name));

	        return RedirectToAction(nameof(Nationalities), "Common");
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNationality(int number)
        {
	        await _commandBus.PublishAsync(CommonCommandFactory.BuildDeleteNationalityCommand(number));

	        return RedirectToAction(nameof(Nationalities), "Common");
        }

		[HttpGet]
        public async Task<IActionResult> Languages()
        {
            IEnumerable<ILanguage> languages = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILanguage>>(CommonQueryFactory.BuildEmptyQuery());

            return GenericCategoryCollectionViewModel.Create("Sprog", "Common", nameof(CreateLanguage), nameof(UpdateLanguage), (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("Common", nameof(DeleteLanguage), urlHelper), languages ?? Array.Empty<ILanguage>(), _commonViewModelConverter)
                .AsView(this);
        }

        [HttpGet]
        public IActionResult CreateLanguage()
        {
            return GenericCategoryViewModel.Create("Opret sprog", "Common", nameof(CreateLanguage), nameof(Languages))
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLanguage(GenericCategoryViewModel genericCategoryViewModel)
        {
	        if (genericCategoryViewModel == null)
	        {
		        return BadRequest();
	        }

	        if (ModelState.IsValid == false)
	        {
		        return BadRequest(ModelState);
	        }

	        await _commandBus.PublishAsync(CommonCommandFactory.BuildCreateLanguageCommand(genericCategoryViewModel.Number, genericCategoryViewModel.Name));

	        return RedirectToAction(nameof(Languages), "Common");
        }

		[HttpGet]
        public async Task<IActionResult> UpdateLanguage(int number)
        {
            ILanguage language = await _queryBus.QueryAsync<IGetLanguageQuery, ILanguage>(CommonQueryFactory.BuildGetLanguageQuery(number));
            if (language == null)
            {
                return BadRequest();
            }

            return GenericCategoryViewModel.Create("Redigér sprog", "Common", nameof(UpdateLanguage), nameof(Languages), language)
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLanguage(GenericCategoryViewModel genericCategoryViewModel)
        {
	        if (genericCategoryViewModel == null)
	        {
		        return BadRequest();
	        }

	        if (ModelState.IsValid == false)
	        {
		        return BadRequest(ModelState);
	        }

	        await _commandBus.PublishAsync(CommonCommandFactory.BuildUpdateLanguageCommand(genericCategoryViewModel.Number, genericCategoryViewModel.Name));

	        return RedirectToAction(nameof(Languages), "Common");
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLanguage(int number)
        {
	        await _commandBus.PublishAsync(CommonCommandFactory.BuildDeleteLanguageCommand(number));

	        return RedirectToAction(nameof(Languages), "Common");
        }

		#endregion
	}
}