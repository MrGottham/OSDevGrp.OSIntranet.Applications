using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class CreditRuleSetBuilder : PostingValueRuleSetBuilderBase, ICreditRuleSetBuilder
{
    #region Constructor

    public CreditRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder, StaticTextKey.Credit, AccountingRuleSetSpecifications.CreditMinValue, AccountingRuleSetSpecifications.CreditMaxValue)
    {
    }

    #endregion
}