using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MinLengthRule : ValidationRuleBase, IMinLengthRule
{
    #region Constructor

    public MinLengthRule(string name, int minLength, string validationError)
        : base(name, validationError)
    {
        MinLength = minLength;
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.MinLengthRule;

    public int MinLength { get; }

    #endregion
}