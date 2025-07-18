using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MaxLengthRule : ValidationRuleBase, IMaxLengthRule
{
    #region Constructor

    public MaxLengthRule(string name, int maxLength, string validationError)
        : base(name, validationError)
    {
        MaxLength = maxLength;
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.MaxLengthRule;

    public int MaxLength { get; }

    #endregion
}