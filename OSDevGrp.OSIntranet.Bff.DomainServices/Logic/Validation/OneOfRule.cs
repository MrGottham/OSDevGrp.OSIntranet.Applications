using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class OneOfRule<TValue> : ValidationRuleBase, IOneOfRule<TValue> where TValue : struct, IComparable<TValue>
{
    #region Constructor

    public OneOfRule(string name, IReadOnlyCollection<TValue> validValues, string validationError)
        : base(name, validationError)
    {
        ValidValues = validValues;
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.OneOfRule;

    public IReadOnlyCollection<TValue> ValidValues { get; }

    #endregion
}