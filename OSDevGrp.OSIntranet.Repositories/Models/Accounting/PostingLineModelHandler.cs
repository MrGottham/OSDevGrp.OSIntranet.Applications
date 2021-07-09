using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class PostingLineModelHandler : ModelHandlerBase<IPostingLine, RepositoryContext, PostingLineModel, string, AccountingIdentificationState>
    {
        #region Private variables

        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly bool _includeCreditInformation;
        private readonly bool _includeBudgetInformation;
        private readonly int? _numberOfPostingLines;
        private readonly bool _applyingPostingLines;

        private readonly AccountingModelHandler _accountingModelHandler;
        private readonly CreditInfoModelHandler _creditInfoModelHandler;
        private readonly BudgetInfoModelHandler _budgetInfoModelHandler;

        private AccountingModel _accountingModel;
        private IReadOnlyCollection<CreditInfoModel> _creditInfoModelCollection;
        private IReadOnlyCollection<BudgetInfoModel> _budgetInfoModelCollection;
        private IReadOnlyCollection<PostingLineModel> _postingLineModelCollection;

        #endregion

        #region Constructor

        public PostingLineModelHandler(RepositoryContext dbContext, IConverter modelConverter, IEventPublisher eventPublisher, DateTime fromDate, DateTime toDate, bool includeCreditInformation, bool includeBudgetInformation, int? numberOfPostingLines = null, bool applyingPostingLines = false) 
            : base(dbContext, modelConverter)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _fromDate = fromDate.Date;
            _toDate = toDate.Date;
            _includeCreditInformation = includeCreditInformation;
            _includeBudgetInformation = includeBudgetInformation;
            _numberOfPostingLines = numberOfPostingLines;
            _applyingPostingLines = applyingPostingLines;

            _accountingModelHandler = new AccountingModelHandler(dbContext, ModelConverter, eventPublisher, _toDate, false, false);

            _creditInfoModelHandler = new CreditInfoModelHandler(dbContext, modelConverter, eventPublisher, _toDate);
            _budgetInfoModelHandler = new BudgetInfoModelHandler(dbContext, modelConverter, eventPublisher, _toDate);
        }

        #endregion

        #region Properties

        protected override DbSet<PostingLineModel> Entities => DbContext.PostingLines;

        protected override Func<IPostingLine, string> PrimaryKey => postingLine => postingLine.Identifier.ToString("D").ToUpper();

        protected override IQueryable<PostingLineModel> Reader => CreateReader(_includeCreditInformation, _includeBudgetInformation, _numberOfPostingLines);

        #endregion

        #region Methods

        internal Task<IEnumerable<IPostingLine>> ReadAsync(int accountingNumber)
        {
            DateTime startDate = _fromDate.Date;
            DateTime endDate = _toDate.AddDays(1).Date;

            return ReadAsync(postingLineModel => postingLineModel.AccountingIdentifier == accountingNumber && postingLineModel.PostingDate >= startDate && postingLineModel.PostingDate < endDate, new AccountingIdentificationState(accountingNumber));
        }

        internal async Task<IEnumerable<PostingLineModel>> ReadAsync(IEnumerable<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            return await Task.WhenAll(postingLineModelCollection.Select(OnReadAsync));
        }

        internal async Task<IReadOnlyCollection<PostingLineModel>> ForAsync(int accountingNumber, bool callPrepareReadAsync = true)
        {
            if (callPrepareReadAsync)
            {
                await PrepareReadAsync(new AccountingIdentificationState(accountingNumber));
            }

            DateTime startDate = _fromDate.Date;
            DateTime endDate = _toDate.AddDays(1).Date;

            IQueryable<PostingLineModel> query = CreateReader(_includeCreditInformation == false, _includeBudgetInformation == false, null)
                .Where(postingLineModel => postingLineModel.AccountingIdentifier == accountingNumber &&
                                           postingLineModel.PostingDate >= startDate &&
                                           postingLineModel.PostingDate < endDate);

            return (await ReadAsync(query)).ToArray();
        }

        internal Task DeleteAsync(IEnumerable<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            if (postingLineModelCollection.Any() == false)
            {
                return Task.CompletedTask;
            }

            throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(PostingLineModel))
                .WithMethodBase(MethodBase.GetCurrentMethod())
                .Build();
        }

        protected override Expression<Func<PostingLineModel, bool>> EntitySelector(string primaryKey) => postingLineModel => postingLineModel.PostingLineIdentification == primaryKey;

        protected override Task<IEnumerable<IPostingLine>> SortAsync(IEnumerable<IPostingLine> postingLineCollection)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            return Task.FromResult(postingLineCollection.OrderByDescending(postingLine => postingLine.PostingDate.Date).ThenByDescending(postingLine => postingLine.SortOrder).AsEnumerable());
        }

        protected override void OnDispose()
        {
            _accountingModelHandler.Dispose();
            _creditInfoModelHandler.Dispose();
            _budgetInfoModelHandler.Dispose();
        }

        protected override async Task<PostingLineModel> OnCreateAsync(IPostingLine postingLine, PostingLineModel postingLineModel)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine))
                .NotNull(postingLineModel, nameof(postingLineModel));

            postingLineModel.Accounting = await DbContext.Accountings.SingleAsync(accountingModel => accountingModel.AccountingIdentifier == postingLine.Accounting.Number);
            postingLineModel.Account = await DbContext.Accounts.SingleAsync(accountModel => accountModel.AccountingIdentifier == postingLine.Accounting.Number && accountModel.AccountNumber == postingLine.Account.AccountNumber);

            if (postingLine.BudgetAccount != null)
            {
                postingLineModel.BudgetAccount = await DbContext.BudgetAccounts.SingleAsync(budgetAccountModel => budgetAccountModel.AccountingIdentifier == postingLine.Accounting.Number && budgetAccountModel.AccountNumber == postingLine.BudgetAccount.AccountNumber);
            }

            if (postingLine.ContactAccount != null)
            {
                postingLineModel.ContactAccount = await DbContext.ContactAccounts.SingleAsync(contactAccountModel => contactAccountModel.AccountingIdentifier == postingLine.Accounting.Number && contactAccountModel.AccountNumber == postingLine.ContactAccount.AccountNumber);
            }

            return postingLineModel;
        }

        protected override async Task PrepareReadAsync(AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            _accountingModel ??= await _accountingModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);

            if (_includeBudgetInformation)
            {
                _creditInfoModelCollection ??= await _creditInfoModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
            }

            if (_includeBudgetInformation)
            {
                _budgetInfoModelCollection ??= await _budgetInfoModelHandler.ForAsync(accountingIdentificationState.AccountingIdentifier);
            }

            if (_numberOfPostingLines.HasValue || _applyingPostingLines)
            {
                _postingLineModelCollection ??= await ForAsync(accountingIdentificationState.AccountingIdentifier, false);
            }
        }

        protected override Task PrepareReadAsync(string primaryKey, AccountingIdentificationState accountingIdentificationState)
        {
            NullGuard.NotNull(accountingIdentificationState, nameof(accountingIdentificationState));

            return PrepareReadAsync(accountingIdentificationState);
        }

        protected override async Task<PostingLineModel> OnReadAsync(PostingLineModel postingLineModel)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel));

            postingLineModel.Accounting ??= _accountingModel;

            postingLineModel.Account = await OnReadAsync(postingLineModel.Account);
            postingLineModel.BudgetAccount = await OnReadAsync(postingLineModel.BudgetAccount);
            postingLineModel.ContactAccount = await OnReadAsync(postingLineModel.ContactAccount);

            return postingLineModel;
        }

        protected override Task OnUpdateAsync(IPostingLine postingLine, PostingLineModel postingLineModel) => throw new NotSupportedException();

        protected override Task<bool> CanDeleteAsync(PostingLineModel postingLineModel) => throw new NotSupportedException();

        protected override Task<PostingLineModel> OnDeleteAsync(PostingLineModel postingLineModel) => throw new NotSupportedException();

        private Task<AccountingModel> OnReadAsync(AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            if (accountingModel.PostingLines == null && _postingLineModelCollection != null)
            {
                accountingModel.ExtractPostingLines(_postingLineModelCollection);
            }

            return Task.FromResult(accountingModel);
        }

        private async Task<AccountModel> OnReadAsync(AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            accountModel.StatusDate = _toDate;
            accountModel.Deletable = false;

            if (accountModel.CreditInfos == null && _creditInfoModelCollection != null)
            {
                accountModel.ExtractCreditInfos(_creditInfoModelCollection);
            }

            if (accountModel.PostingLines == null && _postingLineModelCollection != null)
            {
                accountModel.ExtractPostingLines(_postingLineModelCollection);
            }

            if (accountModel.CreditInfos == null)
            {
                return accountModel;
            }

            accountModel.CreditInfos = (await _creditInfoModelHandler.ReadAsync(accountModel.CreditInfos)).ToList();

            return accountModel;
        }

        private async Task<BudgetAccountModel> OnReadAsync(BudgetAccountModel budgetAccountModel)
        {
            if (budgetAccountModel == null)
            {
                return null;
            }

            budgetAccountModel.StatusDate = _toDate.Date;
            budgetAccountModel.Deletable = false;

            if (budgetAccountModel.BudgetInfos == null && _budgetInfoModelCollection != null)
            {
                budgetAccountModel.ExtractBudgetInfos(_budgetInfoModelCollection);
            }

            if (budgetAccountModel.PostingLines == null && _postingLineModelCollection != null)
            {
                budgetAccountModel.ExtractPostingLines(_postingLineModelCollection);
            }

            if (budgetAccountModel.BudgetInfos == null)
            {
                return budgetAccountModel;
            }

            budgetAccountModel.BudgetInfos = (await _budgetInfoModelHandler.ReadAsync(budgetAccountModel.BudgetInfos)).ToList();

            return budgetAccountModel;
        }

        private Task<ContactAccountModel> OnReadAsync(ContactAccountModel contactAccountModel)
        {
            if (contactAccountModel == null)
            {
                return Task.FromResult<ContactAccountModel>(null);
            }

            contactAccountModel.StatusDate = _toDate.Date;
            contactAccountModel.Deletable = false;

            if (contactAccountModel.PostingLines == null && _postingLineModelCollection != null)
            {
                contactAccountModel.ExtractPostingLines(_postingLineModelCollection);
            }

            return Task.FromResult(contactAccountModel);
        }

        private IQueryable<PostingLineModel> CreateReader(bool includeCreditInformation, bool includeBudgetInformation, int? numberOfPostingLines)
        {
            IQueryable<PostingLineModel> reader = Entities
                .Include(postingLineModel => postingLineModel.Account).ThenInclude(accountModel => accountModel.BasicAccount)
                .Include(postingLineModel => postingLineModel.Account).ThenInclude(accountModel => accountModel.AccountGroup)
                .Include(postingLineModel => postingLineModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.BasicAccount)
                .Include(postingLineModel => postingLineModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.BudgetAccountGroup)
                .Include(postingLineModel => postingLineModel.ContactAccount).ThenInclude(contactAccountModel => contactAccountModel.BasicAccount)
                .Include(postingLineModel => postingLineModel.ContactAccount).ThenInclude(contactAccountModel => contactAccountModel.PaymentTerm);

            if (includeCreditInformation && _creditInfoModelCollection == null)
            {
                reader = reader.Include(postingLineModel => postingLineModel.Account).ThenInclude(accountModel => accountModel.CreditInfos).ThenInclude(creditInfoModel => creditInfoModel.YearMonth);
            }

            if (includeBudgetInformation && _budgetInfoModelCollection == null)
            {
                reader = reader.Include(postingLineModel => postingLineModel.BudgetAccount).ThenInclude(budgetAccountModel => budgetAccountModel.BudgetInfos).ThenInclude(budgetInfoModel => budgetInfoModel.YearMonth);
            }

            if (numberOfPostingLines == null)
            {
                return reader;
            }

            return reader.OrderByDescending(postingLineModel => postingLineModel.PostingDate.Date)
                .ThenByDescending(postingLineModel => postingLineModel.PostingLineIdentifier)
                .Take(numberOfPostingLines.Value);
        }

        #endregion
    }
}