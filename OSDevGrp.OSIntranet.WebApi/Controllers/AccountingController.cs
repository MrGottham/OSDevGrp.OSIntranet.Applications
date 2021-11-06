using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.WebApi.Helpers.Validators;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;

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
        public async Task<ActionResult<IEnumerable<AccountingModel>>> AccountingsAsync()
        {
            IEnumerable<IAccounting> accountings = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(new EmptyQuery());

            IEnumerable<AccountingModel> accountingModels = accountings.AsParallel()
                .Select(accounting => _accountingModelConverter.Convert<IAccounting, AccountingModel>(accounting))
                .OrderBy(accountingModel => accountingModel.Number)
                .ToList();

            return new OkObjectResult(accountingModels);
        }

        [HttpGet("{accountingNumber}")]
        public async Task<ActionResult<AccountingModel>> AccountingAsync(int accountingNumber, DateTimeOffset? statusDate = null)
        {
            IGetAccountingQuery query = new GetAccountingQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IAccounting accounting = await _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(query);

            AccountingModel accountingModel = _accountingModelConverter.Convert<IAccounting, AccountingModel>(accounting);

            return new OkObjectResult(accountingModel);
        }

        [HttpGet("{accountingNumber}/accounts")]
        public async Task<ActionResult<AccountCollectionModel>> AccountsAsync(int accountingNumber, DateTimeOffset? statusDate = null)
        {
            IGetAccountCollectionQuery query = new GetAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IAccountCollection accountCollection = await _queryBus.QueryAsync<IGetAccountCollectionQuery, IAccountCollection>(query);

            AccountCollectionModel accountCollectionModel = _accountingModelConverter.Convert<IAccountCollection, AccountCollectionModel>(accountCollection);

            return new OkObjectResult(accountCollectionModel);
        }

        [HttpGet("{accountingNumber}/accounts/{accountNumber}")]
        public async Task<ActionResult<AccountModel>> AccountAsync(int accountingNumber, string accountNumber, DateTimeOffset? statusDate = null)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(accountNumber))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(accountNumber))
                    .Build();
            }

            IGetAccountQuery query = new GetAccountQuery
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IAccount account = await _queryBus.QueryAsync<IGetAccountQuery, IAccount>(query);
            if (account == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeKnown, nameof(accountNumber))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(accountNumber))
                    .Build();
            }

            AccountModel accountModel = _accountingModelConverter.Convert<IAccount, AccountModel>(account);

            return Ok(accountModel);
        }

        [HttpGet("{accountingNumber}/budgetaccounts")]
        public async Task<ActionResult<BudgetAccountCollectionModel>> BudgetAccountsAsync(int accountingNumber, DateTimeOffset? statusDate = null)
        {
            IGetBudgetAccountCollectionQuery query = new GetBudgetAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IBudgetAccountCollection budgetAccountCollection = await _queryBus.QueryAsync<IGetBudgetAccountCollectionQuery, IBudgetAccountCollection>(query);

            BudgetAccountCollectionModel budgetAccountCollectionModel = _accountingModelConverter.Convert<IBudgetAccountCollection, BudgetAccountCollectionModel>(budgetAccountCollection);

            return new OkObjectResult(budgetAccountCollectionModel);
        }

        [HttpGet("{accountingNumber}/budgetaccounts/{accountNumber}")]
        public async Task<ActionResult<BudgetAccountModel>> BudgetAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset? statusDate = null)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(accountNumber))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(accountNumber))
                    .Build();
            }

            IGetBudgetAccountQuery query = new GetBudgetAccountQuery
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IBudgetAccount budgetAccount = await _queryBus.QueryAsync<IGetBudgetAccountQuery, IBudgetAccount>(query);
            if (budgetAccount == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeKnown, nameof(accountNumber))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(accountNumber))
                    .Build();
            }

            BudgetAccountModel budgetAccountModel = _accountingModelConverter.Convert<IBudgetAccount, BudgetAccountModel>(budgetAccount);

            return Ok(budgetAccountModel);
        }

        [HttpGet("{accountingNumber}/contactaccounts")]
        public async Task<ActionResult<ContactAccountCollectionModel>> ContactAccountsAsync(int accountingNumber, DateTimeOffset? statusDate = null)
        {
            IGetContactAccountCollectionQuery query = new GetContactAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IContactAccountCollection contactAccountCollection = await _queryBus.QueryAsync<IGetContactAccountCollectionQuery, IContactAccountCollection>(query);

            ContactAccountCollectionModel contactAccountCollectionModel = _accountingModelConverter.Convert<IContactAccountCollection, ContactAccountCollectionModel>(contactAccountCollection);

            return new OkObjectResult(contactAccountCollectionModel);
        }

        [HttpGet("{accountingNumber}/debtors")]
        public async Task<ActionResult<ContactAccountCollectionModel>> DebtorsAsync(int accountingNumber, DateTimeOffset? statusDate = null)
        {
            IGetDebtorAccountCollectionQuery query = new GetDebtorAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IContactAccountCollection debtorAccountCollection = await _queryBus.QueryAsync<IGetDebtorAccountCollectionQuery, IContactAccountCollection>(query);

            ContactAccountCollectionModel debtorAccountCollectionModel = _accountingModelConverter.Convert<IContactAccountCollection, ContactAccountCollectionModel>(debtorAccountCollection);

            return new OkObjectResult(debtorAccountCollectionModel);
        }

        [HttpGet("{accountingNumber}/creditors")]
        public async Task<ActionResult<ContactAccountCollectionModel>> CreditorsAsync(int accountingNumber, DateTimeOffset? statusDate = null)
        {
            IGetCreditorAccountCollectionQuery query = new GetCreditorAccountCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IContactAccountCollection creditorAccountCollection = await _queryBus.QueryAsync<IGetCreditorAccountCollectionQuery, IContactAccountCollection>(query);

            ContactAccountCollectionModel creditorAccountCollectionModel = _accountingModelConverter.Convert<IContactAccountCollection, ContactAccountCollectionModel>(creditorAccountCollection);

            return new OkObjectResult(creditorAccountCollectionModel);
        }

        [HttpGet("{accountingNumber}/contactaccounts/{accountNumber}")]
        [HttpGet("{accountingNumber}/debtors/{accountNumber}")]
        [HttpGet("{accountingNumber}/creditors/{accountNumber}")]
        public async Task<ActionResult<ContactAccountModel>> ContactAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset? statusDate = null)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(accountNumber))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(accountNumber))
                    .Build();
            }

            IGetContactAccountQuery query = new GetContactAccountQuery
            {
                AccountingNumber = accountingNumber,
                AccountNumber = accountNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today
            };
            IContactAccount contactAccount = await _queryBus.QueryAsync<IGetContactAccountQuery, IContactAccount>(query);
            if (contactAccount == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueShouldBeKnown, nameof(accountNumber))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(accountNumber))
                    .Build();
            }

            ContactAccountModel contactAccountModel = _accountingModelConverter.Convert<IContactAccount, ContactAccountModel>(contactAccount);

            return Ok(contactAccountModel);
        }

        [HttpGet("{accountingNumber}/postinglines")]
        public async Task<ActionResult<PostingLineCollectionModel>> PostingLinesAsync(int accountingNumber, DateTimeOffset? statusDate = null, int? numberOfPostingLines = null)
        {
            IGetPostingLineCollectionQuery query = new GetPostingLineCollectionQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = statusDate?.LocalDateTime.Date ?? DateTime.Today,
                NumberOfPostingLines = numberOfPostingLines ?? 25
            };
            IPostingLineCollection postingLineCollection = await _queryBus.QueryAsync<IGetPostingLineCollectionQuery, IPostingLineCollection>(query);

            PostingLineCollectionModel postingLineCollectionModel = _accountingModelConverter.Convert<IPostingLineCollection, PostingLineCollectionModel>(postingLineCollection);

            return new OkObjectResult(postingLineCollectionModel);
        }

        [HttpPost("postinglines")]
        public Task<ActionResult<ApplyPostingJournalResultModel>> ApplyPostingJournalAsync([FromBody] ApplyPostingJournalModel applyPostingJournal)
        {
            if (applyPostingJournal == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, nameof(applyPostingJournal))
                    .WithValidatingType(typeof(ApplyPostingJournalModel))
                    .WithValidatingField(nameof(applyPostingJournal))
                    .Build();
            }

            SchemaValidator.Validate(ModelState);

            IApplyPostingJournalCommand applyPostingJournalCommand = _accountingModelConverter.Convert<ApplyPostingJournalModel, ApplyPostingJournalCommand>(applyPostingJournal);

            return ApplyPostingJournalAsync(applyPostingJournalCommand);
        }

        [HttpPost("{accountingNumber}/postinglines")]
        public Task<ActionResult<ApplyPostingJournalResultModel>> ApplyPostingJournalAsync(int accountingNumber, [FromBody] ApplyPostingLineCollectionModel applyPostingLineCollection)
        {
            if (applyPostingLineCollection == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNull, nameof(applyPostingLineCollection))
                    .WithValidatingType(typeof(ApplyPostingLineCollectionModel))
                    .WithValidatingField(nameof(applyPostingLineCollection))
                    .Build();
            }

            SchemaValidator.Validate(ModelState);

            ApplyPostingJournalModel applyPostingJournal = new ApplyPostingJournalModel
            {
                AccountingNumber = accountingNumber,
                ApplyPostingLines = applyPostingLineCollection
            };

            IApplyPostingJournalCommand applyPostingJournalCommand = _accountingModelConverter.Convert<ApplyPostingJournalModel, ApplyPostingJournalCommand>(applyPostingJournal);

            return ApplyPostingJournalAsync(applyPostingJournalCommand);
        }

        [HttpGet("accountgroups")]
        public async Task<ActionResult<IEnumerable<AccountGroupModel>>> AccountGroupsAsync()
        {
            IEnumerable<IAccountGroup> accountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(new EmptyQuery());

            IEnumerable<AccountGroupModel> accountGroupModels = accountGroups.AsParallel()
                .Select(accountGroup => _accountingModelConverter.Convert<IAccountGroup, AccountGroupModel>(accountGroup))
                .OrderBy(accountGroupModel => accountGroupModel.Number)
                .ToList();

            return new OkObjectResult(accountGroupModels);
        }

        [HttpGet("budgetaccountgroups")]
        public async Task<ActionResult<IEnumerable<BudgetAccountGroupModel>>> BudgetAccountGroupsAsync()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(new EmptyQuery());

            IEnumerable<BudgetAccountGroupModel> budgetAccountGroupModels = budgetAccountGroups.AsParallel()
                .Select(budgetAccountGroup => _accountingModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupModel>(budgetAccountGroup))
                .OrderBy(budgetAccountGroupModel => budgetAccountGroupModel.Number)
                .ToList();

            return new OkObjectResult(budgetAccountGroupModels);
        }

        [HttpGet("paymentterms")]
        public async Task<ActionResult<IEnumerable<PaymentTermModel>>> PaymentTermsAsync()
        {
            IEnumerable<IPaymentTerm> paymentTerms = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(new EmptyQuery());

            IEnumerable<PaymentTermModel> paymentTermModels = paymentTerms.AsParallel()
                .Select(paymentTerm => _accountingModelConverter.Convert<IPaymentTerm, PaymentTermModel>(paymentTerm))
                .OrderBy(paymentTermModel => paymentTermModel.Number)
                .ToList();

            return new OkObjectResult(paymentTermModels);
        }

        private async Task<ActionResult<ApplyPostingJournalResultModel>> ApplyPostingJournalAsync(IApplyPostingJournalCommand applyPostingJournalCommand)
        {
            NullGuard.NotNull(applyPostingJournalCommand, nameof(applyPostingJournalCommand));

            IPostingJournalResult postingJournalResult = await _commandBus.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(applyPostingJournalCommand);

            ApplyPostingJournalResultModel applyPostingJournalResultModel = postingJournalResult == null
                ? BuildEmptyApplyPostingJournalResultModel()
                : _accountingModelConverter.Convert<IPostingJournalResult, ApplyPostingJournalResultModel>(postingJournalResult);

            return Ok(applyPostingJournalResultModel);
        }

        private static ApplyPostingJournalResultModel BuildEmptyApplyPostingJournalResultModel()
        {
            return new ApplyPostingJournalResultModel
            {
                PostingLines = new PostingLineCollectionModel(),
                PostingWarnings = new PostingWarningCollectionModel()
            };
        }

        #endregion
    }
}