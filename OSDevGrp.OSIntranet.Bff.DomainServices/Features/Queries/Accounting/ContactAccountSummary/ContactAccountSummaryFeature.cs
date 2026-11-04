using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary;

internal class ContactAccountSummaryFeature : AccountIdentificationFeatureBase<ContactAccountSummaryRequest, ContactAccountSummaryResponse, ContactAccountModel, IContactAccountTexts, IContactAccountTextsBuilder, IEmptyRuleSetBuilder>
{
    #region Constructor

    public ContactAccountSummaryFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IContactAccountTextsBuilder contactAccountTextsBuilder, IEmptyRuleSetBuilder emptyRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, contactAccountTextsBuilder, emptyRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    protected override async Task<ContactAccountModel> GetModelAsync(ContactAccountSummaryRequest request, CancellationToken cancellationToken)
    {
        return await AccountingGateway.GetContactAccountAsync(request.AccountingNumber, request.AccountNumber, request.StatusDate, cancellationToken);
    }

    protected override Task<ContactAccountSummaryResponse> BuildResponseAsync(ContactAccountModel model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IContactAccountTexts contactAccountTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ContactAccountSummaryResponse(model, contactAccountTexts, staticTexts, validationRuleSet));
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(ContactAccountSummaryRequest request, ContactAccountModel model)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.AccountNumberShort, StaticTextKey.AccountNumberShort.DefaultArguments() },
            { StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments() }
        };
    }

    #endregion
}