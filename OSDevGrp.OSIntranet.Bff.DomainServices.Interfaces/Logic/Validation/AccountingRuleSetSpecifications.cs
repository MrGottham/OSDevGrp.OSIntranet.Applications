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

    public const int PostingReferenceMinLength = 1;
    public const int PostingReferenceMaxLength = 16;

    public const int AccountNumberMinLength = 1;
    public const int AccountNumberMaxLength = 16;
    public const string AccountNumberRegexPattern = @"^[0-9A-ZÆØÅ\-+]{1,16}$";

    public const int PostingTextMinLength = 1;
    public const int PostingTextMaxLength = 256;

    public const double DebitMinValue = 0D;
    public const double DebitMaxValue = 99999999D;
    public const double CreditMinValue = 0D;
    public const double CreditMaxValue = 99999999D;
}