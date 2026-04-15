using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MaxValueRule<TValue> : ValidationRuleBase, IMaxValueRule<TValue> where TValue : struct, IComparable<TValue>
{
    #region Constructor

    public MaxValueRule(string name, TValue maxValue, string validationError)
        : base(name, validationError)
    {
        MaxValue = maxValue;
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.MaxValueRule;

    public TValue MaxValue { get; }

    #endregion
}