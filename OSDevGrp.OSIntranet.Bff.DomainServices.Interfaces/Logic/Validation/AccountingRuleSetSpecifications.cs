namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

public static class AccountingRuleSetSpecifications
{
    public const int AccountingNumberMinValue = 1;
    public const int AccountingNumberMaxValue = 99;

    public const int AccountingNameMinLength = 1;
    public const int AccountingNameMaxLength = 256;

    public const int BalanceBelowZeroDebtorsValue = 0;
    public const int BalanceBelowZeroCreditorsValue = 1;

    public const int BackDatingMinValue = 0;
    public const int BackDatingMaxValue = 365;

    public const int PostingLineIdentificationMinLength = 1;
    public const int PostingLineIdentificationMaxLength = 36;
    public const string PostingLineIdentificationRegexPattern = "^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$";
}