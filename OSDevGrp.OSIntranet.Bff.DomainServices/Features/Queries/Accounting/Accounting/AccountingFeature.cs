using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;

internal class AccountingFeature : AccountingIdentificationFeatureBase<AccountingRequest, AccountingResponse, AccountingModel, IAccountingTexts, IAccountingTextsBuilder, IAccountingRuleSetBuilder>
{
    #region Private variables

    private readonly ICommonGateway _commonGateway;
    private readonly IList<LetterHeadIdentificationModel> _letterHeads = new List<LetterHeadIdentificationModel>();

    #endregion

    #region Construcot

    public AccountingFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, ICommonGateway commonGateway, IStaticTextProvider staticTextProvider, IAccountingTextsBuilder accountingTextsBuilder, IAccountingRuleSetBuilder accountingRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, accountingTextsBuilder, accountingRuleSetBuilder)
    {
        _commonGateway = commonGateway;
    }

    #endregion

    #region Methods

    protected override async Task<AccountingModel> GetModelAsync(AccountingRequest request, CancellationToken cancellationToken)
    {
        AccountingModel accountingModel = await AccountingGateway.GetAccountingAsync(request.AccountingNumber, request.StatusDate, cancellationToken);

        await ResolveLetterHeadsAsync(accountingModel, request.SecurityContext, cancellationToken);

        return accountingModel;
    }

    protected override Task<AccountingResponse> BuildResponseAsync(AccountingModel accountingModel, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IAccountingTexts accountingTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.Run(() => new AccountingResponse(accountingModel, accountingTexts, _letterHeads.AsReadOnly(), staticTexts, validationRuleSet), cancellationToken);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountingRequest request, AccountingModel accountingModel)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.UpdateAccounting, StaticTextKey.UpdateAccounting.DefaultArguments() },
            { StaticTextKey.DeleteAccounting, StaticTextKey.DeleteAccounting.DefaultArguments() },
            { StaticTextKey.AccountingDeletionQuestion, new [] {accountingModel.Name} },
            { StaticTextKey.MasterData, StaticTextKey.MasterData.DefaultArguments() },
            { StaticTextKey.AccountingNumber, StaticTextKey.AccountingNumber.DefaultArguments() },
            { StaticTextKey.AccountingName, StaticTextKey.AccountingName.DefaultArguments() },
            { StaticTextKey.LetterHead, StaticTextKey.LetterHead.DefaultArguments() },
            { StaticTextKey.BalanceBelowZero, StaticTextKey.BalanceBelowZero.DefaultArguments() },
            { StaticTextKey.Debtors, StaticTextKey.Debtors.DefaultArguments() },
            { StaticTextKey.Creditors, StaticTextKey.Creditors.DefaultArguments() },
            { StaticTextKey.BackDating, StaticTextKey.BackDating.DefaultArguments() },
            { StaticTextKey.CurrentStatus, StaticTextKey.CurrentStatus.DefaultArguments() },
            { StaticTextKey.IncomeStatement, StaticTextKey.IncomeStatement.DefaultArguments() },
            { StaticTextKey.BalanceSheet, StaticTextKey.BalanceSheet.DefaultArguments() },
            { StaticTextKey.Update, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.Delete, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.ConfirmDeletion, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.DeleteVerificationInfo, StaticTextKey.Update.DefaultArguments() },
            { StaticTextKey.Reset, StaticTextKey.Reset.DefaultArguments() },
            { StaticTextKey.Cancel, StaticTextKey.Cancel.DefaultArguments() }
        };
    }

    private async Task ResolveLetterHeadsAsync(AccountingModel accountingModel, ISecurityContext securityContext, CancellationToken cancellationToken)
    {
        if (PermissionChecker.HasCommonDataAccess(securityContext.User) == false)
        {
            _letterHeads.Add(accountingModel.LetterHead);
            return;
        }

        IEnumerable<LetterHeadModel> letterHeadModels = await _commonGateway.GetLetterHeadsAsync(cancellationToken);
        foreach (LetterHeadModel letterHeadModel in letterHeadModels)
        {
            _letterHeads.Add(new LetterHeadIdentificationModel(letterHeadModel.Name, letterHeadModel.Number));
        }
    }

    #endregion
}