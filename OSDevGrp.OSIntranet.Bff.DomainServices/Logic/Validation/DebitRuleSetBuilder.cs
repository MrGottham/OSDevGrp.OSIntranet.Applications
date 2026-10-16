using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class DebitRuleSetBuilder : PostingValueRuleSetBuilderBase, IDebitRuleSetBuilder
{
    #region Constructor

    public DebitRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder, StaticTextKey.Debit, AccountingRuleSetSpecifications.DebitMinValue, AccountingRuleSetSpecifications.DebitMaxValue)
    {
    }

    #endregion
}