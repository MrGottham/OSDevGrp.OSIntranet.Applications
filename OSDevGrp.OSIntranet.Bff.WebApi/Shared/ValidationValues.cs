namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared;

internal static class ValidationValues
{
    internal const int TitleMinLength = 1;

    internal const int CookieNameMinLength = 1;
    internal const int CookieValueMinLength = 1;
    internal const int DaysUntilCookieExpiryMinValue = 0;
    internal const int DaysUntilCookieExpiryMaxValue = 365;

    internal const int StaticTextKeyMinLength = 1;
    internal const int StaticTextValueMinLength = 1;

    internal const int ValueDisplayerLabelMinLength = 1;
    internal const int ValueDisplayerValueMinLength = 1;

    internal const int ValidationRuleNameMinLength = 1;
    internal const int ValidationErrorMinLength = 1;
    internal const int ValidationValueMinLength = 1;

    internal const int ErrorMessageMinLength = 1;

    internal const string VerificationKeyRegexPattern = @"^[a-z0-9]{128}$";
    internal const string VerificationCodeRegexPattern = @"^[A-Za-z0-9]{6}$";
    internal const int VerificationImageMinLength = 1;
    internal const string VerificationImageRegexPattern = @"^([A-Za-z0-9+\/]{4})*([A-Za-z0-9+\/]{3}=|[A-Za-z0-9+\/]{2}==)?$";

    internal const int NameIdentifierMinLength = 1;
    internal const int NameMinLength = 1;
    internal const int MailAddressMinLength = 1;

    internal const int AccountNumberMinLength = 1;
    internal const int AccountNumberMaxLength = 16;
    internal const string AccountNumberRegexPattern = @"^[0-9A-ZÆØÅ\-+]{1,16}$";
    internal const int AccountNameMinLength = 1;
    internal const int AccountNameMaxLength = 256;

    internal const int BalanceSheetDisplayerHeaderMinLength = 1;
    internal const int BalanceSheetLabelMinLength = 1;
    internal const int BalanceSheetAtStatusDateLabelMinLength = 1;
    internal const int BalanceSheetAtEndOfLastMonthFromStatusDateLabelMinLength = 1;
    internal const int BalanceSheetAtEndOfLastYearFromStatusDateLabelMinLength = 1;
    internal const int AssetsLabelMinLength = 1;
    internal const int LiabilitiesLabelMinLength = 1;
    internal const int FullBalanceSheetLineIdentificationMinLength = 1;
    internal const int FullBalanceSheetLineDescriptionMinLength = 1;
    internal const int CreditAtStatusDateMinLength = 1;
    internal const int BalanceAtStatusDateMinLength = 1;
    internal const int CreditAtEndOfLastMonthFromStatusDateMinLength = 1;
    internal const int BalanceAtEndOfLastMonthFromStatusDateMinLength = 1;
    internal const int CreditAtEndOfLastYearFromStatusDateMinLength = 1;
    internal const int BalanceAtEndOfLastYearFromStatusDateMinLength = 1;
    internal const int BudgetStatementDisplayerHeaderMinLength = 1;
    internal const int ObligeePartiesDisplayerHeaderMinLength = 1;

    internal const int IncomeStatementLabelMinLength = 1;
    internal const int IncomeStatementLineIdentificationMinLength = 1;
    internal const int IncomeStatementLineDescriptionMinLength = 1;
    internal const int MonthOfStatusDateLabelMinLength = 1;
    internal const int LastMonthOfStatusDateLabelMinLength = 1;
    internal const int YearToDateOfStatusDateLabelMinLength = 1;
    internal const int LastYearOfStatusDateLabelMinLength = 1;
    internal const int BudgetLabelMinLength = 1;
    internal const int BudgetMinLength = 1;
    internal const int PostedLabelMinLength = 1;
    internal const int PostedMinLength = 1;
    internal const int AvailableLabelMinLength = 1;
    internal const int AvailableMinLength = 1;

    internal const int LetterHeadNameMinLength = 1;
    internal const int LetterHeadNameMaxLength = 256;
}