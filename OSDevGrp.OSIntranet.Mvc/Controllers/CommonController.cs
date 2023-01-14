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

        #endregion
   }
}