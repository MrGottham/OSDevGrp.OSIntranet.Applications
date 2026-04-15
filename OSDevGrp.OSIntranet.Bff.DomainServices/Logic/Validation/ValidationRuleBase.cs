using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal abstract class ValidationRuleBase : IValidationRule
{
    #region Constructor

    protected ValidationRuleBase(string name, string validationError)
    {
        Name = name;
        ValidationError = validationError;
    }

    #endregion

    #region Properties

    public string Name { get; }

    public abstract ValidationRuleType RuleType { get; }

    public string ValidationError { get; }

    #endregion
}