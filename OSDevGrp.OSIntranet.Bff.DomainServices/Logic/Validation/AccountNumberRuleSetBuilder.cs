using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class AccountNumberRuleSetBuilder : AccountNumberRuleSetBuilderBase, IAccountNumberRuleSetBuilder
{
    #region Constructor

    public AccountNumberRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder, StaticTextKey.Account, true)
    {
    }

    #endregion
}