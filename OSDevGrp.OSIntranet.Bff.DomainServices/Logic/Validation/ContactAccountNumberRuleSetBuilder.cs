using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class ContactAccountNumberRuleSetBuilder : AccountNumberRuleSetBuilderBase, IContactAccountNumberRuleSetBuilder
{
    #region Constructor

    public ContactAccountNumberRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder, StaticTextKey.ContactAccount, false)
    {
    }

    #endregion
}