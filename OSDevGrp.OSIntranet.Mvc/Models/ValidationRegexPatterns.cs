namespace OSDevGrp.OSIntranet.Mvc.Models
{
    public static class ValidationRegexPatterns
    {
        public const string MailAddressRegexPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string PhoneNumberPrefixRegexPattern = @"\+[0-9]{1,3}";
        public const string PhoneNumberRegexPattern = @"^[\+]?[0-9\s]+$";
        public const string CountryCodeRegexPattern = @"[A-Z]{1,4}";
        public const string PostalCodeRegexPattern = @"[0-9]{1,16}";
        public const string UrlRegexPattern = @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$";
        public const string AccountNumberRegexPattern = @"[0-9A-Z\-+]{1,16}";
    }
}