using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary;

internal class BudgetAccountSummaryFeature : AccountIdentificationFeatureBase<BudgetAccountSummaryRequest, BudgetAccountSummaryResponse, BudgetAccountModel, IBudgetAccountTexts, IBudgetAccountTextsBuilder, IEmptyRuleSetBuilder>
{
    #region Constructor

    public BudgetAccountSummaryFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IBudgetAccountTextsBuilder budgetAccountTextsBuilder, IEmptyRuleSetBuilder emptyRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, budgetAccountTextsBuilder, emptyRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    protected override async Task<BudgetAccountModel> GetModelAsync(BudgetAccountSummaryRequest request, CancellationToken cancellationToken)
    {
        return await AccountingGateway.GetBudgetAccountAsync(request.AccountingNumber, request.AccountNumber, request.StatusDate, cancellationToken);
    }

    protected override Task<BudgetAccountSummaryResponse> BuildResponseAsync(BudgetAccountModel model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IBudgetAccountTexts budgetAccountTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.FromResult(new BudgetAccountSummaryResponse(model, budgetAccountTexts, staticTexts, validationRuleSet));
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(BudgetAccountSummaryRequest request, BudgetAccountModel model)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.AccountNumberShort, StaticTextKey.AccountNumberShort.DefaultArguments() },
            { StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments() }
        };
    }

    #endregion
}