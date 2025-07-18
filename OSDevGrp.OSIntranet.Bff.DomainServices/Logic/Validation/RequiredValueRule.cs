using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class RequiredValueRule : ValidationRuleBase, IRequiredValueRule
{
    #region Constructor

    public RequiredValueRule(string name, string validationError)
        : base(name, validationError)
    {
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.RequiredValueRule;

    #endregion
}