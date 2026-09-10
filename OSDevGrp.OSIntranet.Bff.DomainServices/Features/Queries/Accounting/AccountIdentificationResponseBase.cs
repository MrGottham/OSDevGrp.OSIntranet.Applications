using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;

public abstract class AccountIdentificationResponseBase<TModel, TDynamicTexts> : AccountingIdentificationResponseBase<TModel, TDynamicTexts> where TModel : class where TDynamicTexts : IDynamicTexts
{
    #region Constructor

    protected AccountIdentificationResponseBase(TModel model, TDynamicTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, dynamicTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion

    #region Properties

    #endregion
}