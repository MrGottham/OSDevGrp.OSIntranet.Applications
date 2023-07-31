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
    }
}