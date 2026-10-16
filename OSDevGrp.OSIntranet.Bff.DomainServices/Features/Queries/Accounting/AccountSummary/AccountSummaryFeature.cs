using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary;

internal class AccountSummaryFeature : AccountIdentificationFeatureBase<AccountSummaryRequest, AccountSummaryResponse, AccountModel, IAccountTexts, IAccountTextsBuilder, IEmptyRuleSetBuilder>
{
    #region Constructor

    public AccountSummaryFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IAccountTextsBuilder accountTextsBuilder, IEmptyRuleSetBuilder emptyRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, accountTextsBuilder, emptyRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    protected override async Task<AccountModel> GetModelAsync(AccountSummaryRequest request, CancellationToken cancellationToken)
    {
        return await AccountingGateway.GetAccountAsync(request.AccountingNumber, request.AccountNumber, request.StatusDate, cancellationToken);
    }

    protected override Task<AccountSummaryResponse> BuildResponseAsync(AccountModel model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IAccountTexts accountTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.FromResult(new AccountSummaryResponse(model, accountTexts, staticTexts, validationRuleSet));
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountSummaryRequest request, AccountModel model)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.AccountNumberShort, StaticTextKey.AccountNumberShort.DefaultArguments() },
            { StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments() }
        };
    }

    #endregion
}