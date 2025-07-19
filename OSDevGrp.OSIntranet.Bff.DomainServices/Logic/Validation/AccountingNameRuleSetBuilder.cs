using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;

internal class AccountingNameRuleSetBuilder : ValidationRuleSetBuilderBase, IAccountingNameRuleSetBuilder
{
    #region Constructor

    public AccountingNameRuleSetBuilder(IExtendedValidationRuleSetBuilder extendedValidationRuleSetBuilder)
        : base(extendedValidationRuleSetBuilder)    
    {
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyCollection<IValidationRule>> BuildAsync(IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await ExtendedValidationRuleSetBuilder.WithRequiredValueRule(StaticTextKey.AccountingName)
            .WithMinLengthRule(StaticTextKey.AccountingName, AccountingRuleSetSpecifications.AccountingNameMinLength)
            .WithMaxLengthRule(StaticTextKey.AccountingName, AccountingRuleSetSpecifications.AccountingNameMaxLength)
            .BuildAsync(formatProvider, cancellationToken);
    }

    #endregion
}