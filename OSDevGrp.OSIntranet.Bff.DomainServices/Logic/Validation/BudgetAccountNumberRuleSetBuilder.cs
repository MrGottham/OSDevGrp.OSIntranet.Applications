using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class BudgetAccountNumberRuleSetBuilder : AccountNumberRuleSetBuilderBase, IBudgetAccountNumberRuleSetBuilder
{
    #region Constructor

    public BudgetAccountNumberRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder, StaticTextKey.BudgetAccount, false)
    {
    }

    #endregion
}