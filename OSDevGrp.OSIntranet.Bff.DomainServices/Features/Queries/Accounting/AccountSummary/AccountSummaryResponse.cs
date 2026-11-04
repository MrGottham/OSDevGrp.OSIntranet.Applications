using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary;

public class AccountSummaryResponse : AccountIdentificationResponseBase<AccountModel, IAccountTexts>
{
    #region Constructor

    public AccountSummaryResponse(AccountModel model, IAccountTexts accountTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, accountTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion

    #region Properties

    public AccountModel Account => Model;

    #endregion
}