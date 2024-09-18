namespace OSDevGrp.OSIntranet.BusinessLogic.Tests
{
    internal static class RegexTestHelper
    {
        internal const string CountryCodeRegexPattern = "[A-Z]{1,4}";
        internal const string PostalCodeRegexPattern = "[0-9]{1,16}";
        internal const string PhonePrefixRegexPattern = @"\+[0-9]{1,3}";
        internal const string PhoneNumberRegexPattern = @"^[\+]?[0-9\s]+$";
        internal const string MailAddressRegexPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        internal const string UrlRegexPattern = @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$";
        internal const string AccountNumberPattern = @"[0-9A-Z\-+]{1,16}";
        internal const string InternationalStandardBookNumberPattern = @"^(?:ISBN(?:-1[03])?:?\s)?(?=[0-9X]{10}$|(?=(?:[0-9]+[-\s]){3})[-\s0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[-\s]){4})[-\s0-9]{17}$)(?:97[89][-\s]?)?[0-9]{1,5}[-\s]?[0-9]+[-\s]?[0-9]+[-\s]?[0-9X]$";
        internal const string Base64Pattern = @"^([A-Za-z0-9+\/]{4})*([A-Za-z0-9+\/]{3}=|[A-Za-z0-9+\/]{2}==)?$";
        internal const string ResponseTypeForAuthorizationCodeFlowPattern = "^(code){1}$";
        internal const string JwtTokenPattern = @"^[A-Za-z0-9_-]{2,}(?:\.[A-Za-z0-9_-]{2,}){2}$";
        internal const string GuidPattern = "^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$";
        internal const string Iso8601DateTimePattern = @"^\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z)$";
    }
}