using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;

internal class PostingJournalFeature : AccountingIdentificationFeatureBase<PostingJournalRequest, PostingJournalResponse, Tuple<ApplyPostingJournalModel, Predicate<int>>, IPostingJournalTexts, IPostingJournalTextsBuilder, IPostingJournalRuleSetBuilder>
{
    #region Constructor

    public PostingJournalFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IPostingJournalTextsBuilder postingJournalTextsBuilder, IPostingJournalRuleSetBuilder postingJournalRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, postingJournalTextsBuilder, postingJournalRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    public override Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, PostingJournalRequest request, CancellationToken cancellationToken)
    {
        return Task.Run(() => VerifyPermission(securityContext.User, request.AccountingNumber), cancellationToken);
    }

    protected override async Task<Tuple<ApplyPostingJournalModel, Predicate<int>>> GetModelAsync(PostingJournalRequest request, CancellationToken cancellationToken)
    {
        ApplyPostingJournalModel postingJournalModel = await AccountingGateway.GetPostingJournalAsync(request.AccountingNumber, cancellationToken);
        Predicate<int> modifiablePredicate = accountingNumber => PermissionChecker.IsAccountingModifier(request.SecurityContext.User, accountingNumber);
        return new Tuple<ApplyPostingJournalModel, Predicate<int>>(postingJournalModel, modifiablePredicate);
    }

    protected override Task<PostingJournalResponse> BuildResponseAsync(Tuple<ApplyPostingJournalModel, Predicate<int>> model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IPostingJournalTexts dynamicTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PostingJournalResponse(model, dynamicTexts, staticTexts, validationRuleSet));
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(PostingJournalRequest request, Tuple<ApplyPostingJournalModel, Predicate<int>> model)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.PostingJournal, StaticTextKey.PostingJournal.DefaultArguments() },
            { StaticTextKey.PostingDate, StaticTextKey.PostingDate.DefaultArguments() },
            { StaticTextKey.PostingReference, StaticTextKey.PostingReference.DefaultArguments() },
            { StaticTextKey.Account, StaticTextKey.Account.DefaultArguments() },
            { StaticTextKey.PostingText, StaticTextKey.PostingText.DefaultArguments() },
            { StaticTextKey.BudgetAccount, StaticTextKey.BudgetAccount.DefaultArguments() },
            { StaticTextKey.Debit, StaticTextKey.Debit.DefaultArguments() },
            { StaticTextKey.Credit, StaticTextKey.Credit.DefaultArguments() },
            { StaticTextKey.ContactAccount, StaticTextKey.ContactAccount.DefaultArguments() },
            { StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments() },
            { StaticTextKey.Posted, StaticTextKey.Posted.DefaultArguments() },
            { StaticTextKey.Available, StaticTextKey.Available.DefaultArguments() },
            { StaticTextKey.Balance, StaticTextKey.Balance.DefaultArguments() },
            { StaticTextKey.PostingValue, StaticTextKey.PostingValue.DefaultArguments() },
            { StaticTextKey.AddPostingJournalLine, StaticTextKey.AddPostingJournalLine.DefaultArguments() },
            { StaticTextKey.UpdatePostingJournalLine, StaticTextKey.UpdatePostingJournalLine.DefaultArguments() },
            { StaticTextKey.DeletePostingJournalLine, StaticTextKey.DeletePostingJournalLine.DefaultArguments() },
            { StaticTextKey.PostingJournalLineDeletionQuestion, StaticTextKey.PostingJournalLineDeletionQuestion.DefaultArguments() },
            { StaticTextKey.Create, StaticTextKey.Create.DefaultArguments() },
            { StaticTextKey.Update, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.Delete, StaticTextKey.Delete.DefaultArguments() },
            { StaticTextKey.ConfirmDeletion, StaticTextKey.ConfirmDeletion.DefaultArguments() },
            { StaticTextKey.DeleteVerificationInfo, StaticTextKey.DeleteVerificationInfo.DefaultArguments() },
            { StaticTextKey.Reset, StaticTextKey.Reset.DefaultArguments() },
            { StaticTextKey.Cancel, StaticTextKey.Cancel.DefaultArguments() }
        };
    }

    private bool VerifyPermission(ClaimsPrincipal user, int accountingNumber)
    {
        return PermissionChecker.IsAuthenticated(user) && PermissionChecker.HasAccountingAccess(user) && PermissionChecker.IsAccountingModifier(user, accountingNumber);
    }

    #endregion
}