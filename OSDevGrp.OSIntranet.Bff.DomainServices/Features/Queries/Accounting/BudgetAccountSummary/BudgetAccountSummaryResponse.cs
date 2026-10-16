using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary;

public class BudgetAccountSummaryResponse : AccountIdentificationResponseBase<BudgetAccountModel, IBudgetAccountTexts>
{
    #region Constructor

    public BudgetAccountSummaryResponse(BudgetAccountModel model, IBudgetAccountTexts budgetAccountTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, budgetAccountTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion

    #region Properties

    public BudgetAccountModel BudgetAccount => Model;

    #endregion
}