using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;

internal class AccountingFeature : AccountingIdentificationFeatureBase<AccountingRequest, AccountingResponse, Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, ApplyPostingJournalModel, IReadOnlyCollection<LetterHeadIdentificationModel>>, IAccountingTexts, IAccountingTextsBuilder, IAccountingRuleSetBuilder>
{
    #region Private variables

    private readonly ICommonGateway _commonGateway;

    #endregion

    #region Construcot

    public AccountingFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, ICommonGateway commonGateway, IStaticTextProvider staticTextProvider, IAccountingTextsBuilder accountingTextsBuilder, IAccountingRuleSetBuilder accountingRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, accountingTextsBuilder, accountingRuleSetBuilder)
    {
        _commonGateway = commonGateway;
    }

    #endregion

    #region Methods

    protected override async Task<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, ApplyPostingJournalModel, IReadOnlyCollection<LetterHeadIdentificationModel>>> GetModelAsync(AccountingRequest request, CancellationToken cancellationToken)
    {
        AccountingModel? accountingModel = null;
        IReadOnlyCollection<PostingLineModel> postingLineModels = [];
        ApplyPostingJournalModel? postingJournalModel = null;

        Task getAccountingTask = AccountingGateway.GetAccountingAsync(request.AccountingNumber, request.StatusDate, cancellationToken).ContinueWith(task => accountingModel = task.Result, cancellationToken);
        Task getPostingLinesTask = request.NumberOfPostingLines > 0
            ? AccountingGateway.GetPostingLinesAsync(request.AccountingNumber, request.StatusDate, request.NumberOfPostingLines, _ => true, cancellationToken).ContinueWith(task => postingLineModels = task.Result.ToArray(), cancellationToken)
            : Task.CompletedTask;
        Task getPostingJournalTask = PermissionChecker.IsAccountingModifier(request.SecurityContext.User, request.AccountingNumber)
            ? AccountingGateway.GetPostingJournalAsync(request.AccountingNumber, cancellationToken).ContinueWith(task => postingJournalModel = task.Result, cancellationToken)
            : Task.Run(() => postingJournalModel = new ApplyPostingJournalModel(request.AccountingNumber, []), cancellationToken);

        await Task.WhenAll(getAccountingTask, getPostingLinesTask, getPostingJournalTask);

        IReadOnlyCollection<LetterHeadIdentificationModel> letterHeadIdentificationModels = await ResolveLetterHeadsAsync(accountingModel!, request.SecurityContext, cancellationToken);

        return Tuple.Create(accountingModel!, postingLineModels, postingJournalModel!, letterHeadIdentificationModels);
    }

    protected override Task<AccountingResponse> BuildResponseAsync(Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, ApplyPostingJournalModel, IReadOnlyCollection<LetterHeadIdentificationModel>> model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IAccountingTexts accountingTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.Run(() => new AccountingResponse(model, accountingTexts, staticTexts, validationRuleSet), cancellationToken);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountingRequest request, Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, ApplyPostingJournalModel, IReadOnlyCollection<LetterHeadIdentificationModel>> model)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.UpdateAccounting, StaticTextKey.UpdateAccounting.DefaultArguments() },
            { StaticTextKey.DeleteAccounting, StaticTextKey.DeleteAccounting.DefaultArguments() },
            { StaticTextKey.AccountingDeletionQuestion, new [] {model.Item1.Name} },
            { StaticTextKey.AccountDeletionQuestion, StaticTextKey.AccountDeletionQuestion.DefaultArguments() },
            { StaticTextKey.BudgetAccountDeletionQuestion, StaticTextKey.BudgetAccountDeletionQuestion.DefaultArguments() },
            { StaticTextKey.ContactAccountDeletionQuestion, StaticTextKey.ContactAccountDeletionQuestion.DefaultArguments() },
            { StaticTextKey.MasterData, StaticTextKey.MasterData.DefaultArguments() },
            { StaticTextKey.AccountingNumber, StaticTextKey.AccountingNumber.DefaultArguments() },
            { StaticTextKey.AccountingName, StaticTextKey.AccountingName.DefaultArguments() },
            { StaticTextKey.LetterHead, StaticTextKey.LetterHead.DefaultArguments() },
            { StaticTextKey.BalanceBelowZero, StaticTextKey.BalanceBelowZero.DefaultArguments() },
            { StaticTextKey.Debtors, StaticTextKey.Debtors.DefaultArguments() },
            { StaticTextKey.Creditors, StaticTextKey.Creditors.DefaultArguments() },
            { StaticTextKey.BackDating, StaticTextKey.BackDating.DefaultArguments() },
            { StaticTextKey.CurrentStatus, StaticTextKey.CurrentStatus.DefaultArguments() },
            { StaticTextKey.Bookkeeping, StaticTextKey.Bookkeeping.DefaultArguments() },
            { StaticTextKey.AddPostingJournalLine, StaticTextKey.AddPostingJournalLine.DefaultArguments() },
            { StaticTextKey.UpdatePostingJournalLine, StaticTextKey.UpdatePostingJournalLine.DefaultArguments() },
            { StaticTextKey.DeletePostingJournalLine, StaticTextKey.DeletePostingJournalLine.DefaultArguments() },
            { StaticTextKey.PostingJournalLineDeletionQuestion, StaticTextKey.PostingJournalLineDeletionQuestion.DefaultArguments() },
            { StaticTextKey.IncomeStatement, StaticTextKey.IncomeStatement.DefaultArguments() },
            { StaticTextKey.BalanceSheet, StaticTextKey.BalanceSheet.DefaultArguments() },
            { StaticTextKey.Accounts, StaticTextKey.Accounts.DefaultArguments() },
            { StaticTextKey.BudgetAccounts, StaticTextKey.BudgetAccounts.DefaultArguments() },
            { StaticTextKey.ContactAccounts, StaticTextKey.ContactAccounts.DefaultArguments() },
            { StaticTextKey.Create, StaticTextKey.Create.DefaultArguments() },
            { StaticTextKey.Update, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.Delete, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.ConfirmDeletion, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.DeleteVerificationInfo, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.Reset, StaticTextKey.Reset.DefaultArguments() },
            { StaticTextKey.Cancel, StaticTextKey.Cancel.DefaultArguments() }
        };
    }

    private async Task<IReadOnlyCollection<LetterHeadIdentificationModel>> ResolveLetterHeadsAsync(AccountingModel accountingModel, ISecurityContext securityContext, CancellationToken cancellationToken)
    {
        if (PermissionChecker.HasCommonDataAccess(securityContext.User) == false)
        {
            return [accountingModel.LetterHead];
        }

        return (await _commonGateway.GetLetterHeadsAsync(cancellationToken))
            .Select(letterHeadModel => new LetterHeadIdentificationModel(letterHeadModel.Name, letterHeadModel.Number))
            .ToArray();
    }

    #endregion
}