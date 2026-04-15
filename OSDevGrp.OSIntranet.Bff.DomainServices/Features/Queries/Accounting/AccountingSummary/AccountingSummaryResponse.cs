using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;

public class AccountingSummaryResponse : AccountingIdentificationResponseBase<AccountingModel, IAccountingTexts>
{
    #region Constructor

    public AccountingSummaryResponse(AccountingModel model, IAccountingTexts accountingTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet) 
        : base(model, accountingTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion
}