using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
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

        [HttpGet]
        [HttpGet("accountings")]
        [ProducesResponseType(typeof(IEnumerable<AccountingModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<AccountingModel>>> AccountingsAsync()
        {
            try
            {
                IEnumerable<IAccounting> accountings = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(new EmptyQuery());

                IEnumerable<AccountingModel> accountingModels = accountings.AsParallel()
                    .Select(accounting => _accountingModelConverter.Convert<IAccounting, AccountingModel>(accounting))
                    .OrderBy(accountingModel => accountingModel.Number)
                    .ToList();

                return new OkObjectResult(accountingModels);
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

        [HttpGet("{accountingNumber}")]
        [ProducesResponseType(typeof(AccountingModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AccountingModel>> AccountingAsync(int accountingNumber, DateTime? statusDate = null)
        {
            try
            {
                IGetAccountingQuery query = new GetAccountingQuery
                {
                    AccountingNumber = accountingNumber,
                    StatusDate = statusDate?.Date ?? DateTime.Today
                };
                IAccounting accounting = await _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(query);

                AccountingModel accountingModel = _accountingModelConverter.Convert<IAccounting, AccountingModel>(accounting);

                return new OkObjectResult(accountingModel);
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

        [HttpGet("accountgroups")]
        [ProducesResponseType(typeof(IEnumerable<AccountGroupModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(typeof(IEnumerable<BudgetAccountGroupModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(typeof(IEnumerable<PaymentTermModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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