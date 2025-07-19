using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;

public abstract class AccountingIdentificationResponseBase<TModel, TDynamicTexts> : PageResponseBase where TModel : class where TDynamicTexts : IDynamicTexts
{
    #region Constructor

    protected AccountingIdentificationResponseBase(TModel model, TDynamicTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(staticTexts)
    {
        Model = model;
        DynamicTexts = dynamicTexts;
        ValidationRuleSet = validationRuleSet;
    }

    #endregion

    #region Properties

    public TModel Model { get; set; }

    public TDynamicTexts DynamicTexts { get; set; }

    public IReadOnlyCollection<IValidationRule> ValidationRuleSet { get; }

    #endregion
}