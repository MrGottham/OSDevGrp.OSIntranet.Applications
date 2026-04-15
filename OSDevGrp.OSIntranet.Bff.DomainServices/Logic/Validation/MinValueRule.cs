using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class MinValueRule<TValue> : ValidationRuleBase, IMinValueRule<TValue> where TValue : struct, IComparable<TValue>
{
    #region Constructor

    public MinValueRule(string name, TValue minValue, string validationError)
        : base(name, validationError)
    {
        MinValue = minValue;
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.MinValueRule;

    public TValue MinValue { get; }

    #endregion
}