using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountIdentificationFeatureBase;

public class MyAccountIdentificationResponse : AccountIdentificationResponseBase<object, IDynamicTexts>
{
    #region Constructor

    public MyAccountIdentificationResponse(object model, IDynamicTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, dynamicTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion
}