using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class OneOfRule<TValue> : ValidationRuleBase, IOneOfRule<TValue> where TValue : IComparable<TValue>
{
    #region Constructor

    public OneOfRule(string name, IReadOnlyCollection<IValueSpecification<TValue>> validValues, string validationError)
        : base(name, validationError)
    {
        ValidValues = validValues;
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.OneOfRule;

    public IReadOnlyCollection<IValueSpecification<TValue>> ValidValues { get; }

    #endregion
}