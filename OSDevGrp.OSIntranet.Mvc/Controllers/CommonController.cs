using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "CommonData")]
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
            IEnumerable<ILetterHead> lettterHeads = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(new EmptyQuery());

            IEnumerable<LetterHeadViewModel> letterHeadViewModels = lettterHeads.AsParallel()
                .Select(letterHead => _commonViewModelConverter.Convert<ILetterHead, LetterHeadViewModel>(letterHead))
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

        #endregion
   }
}