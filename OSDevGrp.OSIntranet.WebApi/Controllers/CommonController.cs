using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.WebApi.Models.Common;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
    [Authorize(Policy = "CommonData")]
    [ApiVersion("1.0")]
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        #region Private variables

        private readonly IQueryBus _queryBus;
        private readonly IConverter _commonModelConverter = new CommonModelConverter();

        #endregion

        #region Constructor

        public CommonController(IQueryBus queryBus)
        {
            NullGuard.NotNull(queryBus, nameof(queryBus));

            _queryBus = queryBus;
        }

        #endregion

        #region Methods

        [HttpGet("letterheads")]
        public async Task<ActionResult<IEnumerable<LetterHeadModel>>> LetterHeadsAsync()
        {
            IEnumerable<ILetterHead> letterHeads = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(new EmptyQuery());

            IEnumerable<LetterHeadModel> letterHeadModels = letterHeads.AsParallel()
                .Select(letterHead => _commonModelConverter.Convert<ILetterHead, LetterHeadModel>(letterHead))
                .OrderBy(letterHeadModel => letterHeadModel.Number)
                .ToList();

            return new OkObjectResult(letterHeadModels);
        }

        #endregion
    }
}