using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;

internal class AccountingSummaryFeature : AccountingIdentificationFeatureBase<AccountingSummaryRequest, AccountingSummaryResponse, AccountingModel, IAccountingTexts, IAccountingTextsBuilder, IEmptyRuleSetBuilder>
{
    #region Constructor

    public AccountingSummaryFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IAccountingTextsBuilder accountingTextsBuilder, IEmptyRuleSetBuilder emptyRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, accountingTextsBuilder, emptyRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    protected override Task<AccountingModel> GetModelAsync(AccountingSummaryRequest request, CancellationToken cancellationToken)
    {
        return AccountingGateway.GetAccountingAsync(request.AccountingNumber, request.StatusDate, cancellationToken);
    }

    protected override Task<AccountingSummaryResponse> BuildResponseAsync(AccountingModel accountingModel, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IAccountingTexts accountingTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.FromResult(new AccountingSummaryResponse(accountingModel, accountingTexts, staticTexts, validationRuleSet));
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountingSummaryRequest request, AccountingModel accountingModel)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.AccountingNumber, StaticTextKey.AccountingNumber.DefaultArguments() },
            { StaticTextKey.AccountingName, StaticTextKey.AccountingName.DefaultArguments() }
        };
    }

    #endregion
}