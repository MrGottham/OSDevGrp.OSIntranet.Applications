using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal abstract class ValidationRuleFactoryBase : IValidationRuleFactory
{
    #region Constructor

    protected ValidationRuleFactoryBase(IStaticTextProvider staticTextProvider)
    {
        StaticTextProvider = staticTextProvider;
    }

    #endregion

    #region Properties

    protected IStaticTextProvider StaticTextProvider { get; }

    #endregion
}