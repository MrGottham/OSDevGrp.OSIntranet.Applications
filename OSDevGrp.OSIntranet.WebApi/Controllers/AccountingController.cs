using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.WebApi.Helpers.Controllers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using OSDevGrp.OSIntranet.WebApi.Models.Core;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
    [Authorize(Policy = "Accounting")]
    [ApiVersion("0.1")]
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class AccountingController : ControllerBase
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IConverter _accountingModelConverter = new AccountingModelConverter();
        private readonly IConverter _coreModelConverter = new CoreModelConverter();

        #endregion

        #region Constructor

        public AccountingController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region Methods

        [HttpGet("accountgroups")]
        public async Task<ActionResult<IEnumerable<AccountGroupModel>>> AccountGroupsAsync()
        {
            try
            {
                IEnumerable<IAccountGroup> accountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(new EmptyQuery());

                IEnumerable<AccountGroupModel> accountGroupModels = accountGroups.AsParallel()
                    .Select(accountGroup => _accountingModelConverter.Convert<IAccountGroup, AccountGroupModel>(accountGroup))
                    .OrderBy(accountGroupModel => accountGroupModel.Number)
                    .ToList();

                return new OkObjectResult(accountGroupModels);
            }
            catch (IntranetExceptionBase ex)
            {
                return BadRequest(_coreModelConverter.Convert<IntranetExceptionBase, ErrorModel>(ex));
            }
            catch (AggregateException ex)
            {
                return BadRequest(ex.ToErrorModel(_coreModelConverter));
            }
        }

        [HttpGet("budgetaccountgroups")]
        public async Task<ActionResult<IEnumerable<BudgetAccountGroupModel>>> BudgetAccountGroupsAsync()
        {
            try
            {
                IEnumerable<IBudgetAccountGroup> budgetAccountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(new EmptyQuery());

                IEnumerable<BudgetAccountGroupModel> budgetAccountGroupModels = budgetAccountGroups.AsParallel()
                    .Select(budgetAccountGroup => _accountingModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupModel>(budgetAccountGroup))
                    .OrderBy(budgetAccountGroupModel => budgetAccountGroupModel.Number)
                    .ToList();

                return new OkObjectResult(budgetAccountGroupModels);
            }
            catch (IntranetExceptionBase ex)
            {
                return BadRequest(_coreModelConverter.Convert<IntranetExceptionBase, ErrorModel>(ex));
            }
            catch (AggregateException ex)
            {
                return BadRequest(ex.ToErrorModel(_coreModelConverter));
            }
        }

        [HttpGet("paymentterms")]
        public async Task<ActionResult<IEnumerable<PaymentTermModel>>> PaymentTermsAsync()
        {
            try
            {
                IEnumerable<IPaymentTerm> paymentTerms = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(new EmptyQuery());

                IEnumerable<PaymentTermModel> paymentTermModels = paymentTerms.AsParallel()
                    .Select(paymentTerm => _accountingModelConverter.Convert<IPaymentTerm, PaymentTermModel>(paymentTerm))
                    .OrderBy(paymentTermModel => paymentTermModel.Number)
                    .ToList();

                return new OkObjectResult(paymentTermModels);
            }
            catch (IntranetExceptionBase ex)
            {
                return BadRequest(_coreModelConverter.Convert<IntranetExceptionBase, ErrorModel>(ex));
            }
            catch (AggregateException ex)
            {
                return BadRequest(ex.ToErrorModel(_coreModelConverter));
            }
        }

        #endregion
    }
}
