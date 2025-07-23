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

    internal const int NameIdentifierMinLength = 1;
    internal const int NameMinLength = 1;
    internal const int MailAddressMinLength = 1;

    internal const int AccountNumberMinLength = 1;
    internal const int AccountNumberMaxLength = 16;
    internal const string AccountNumberRegexPattern = @"^[0-9A-ZÆØÅ\-+]{1,16}$";
    internal const int AccountNameMinLength = 1;
    internal const int AccountNameMaxLength = 256;

    internal const int LetterHeadNameMinLength = 1;
    internal const int LetterHeadNameMaxLength = 256;
}