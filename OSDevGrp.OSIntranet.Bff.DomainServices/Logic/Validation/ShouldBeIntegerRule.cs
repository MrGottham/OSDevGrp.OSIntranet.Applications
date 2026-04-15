using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class ShouldBeIntegerRule : ValidationRuleBase, IShouldBeIntegerRule
{
    #region Constructor

    public ShouldBeIntegerRule(string name, string validationError)
        : base(name, validationError)
    {
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.ShouldBeIntegerRule;

    #endregion
}