using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class PatternRule : ValidationRuleBase, IPatternRule
{
    #region Constructor

    public PatternRule(string name, Regex pattern, string validationError)
        : base(name, validationError)
    {
        Pattern = pattern;
    }

    #endregion

    #region Properties

    public override ValidationRuleType RuleType => ValidationRuleType.PatternRule;

    public Regex Pattern { get; }

    #endregion
}