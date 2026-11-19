namespace OSDevGrp.OSIntranet.WebApi.Models
{
    public static class ValidationRegexPatterns
    {
        public const string MailAddressRegexPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string PhoneNumberRegexPattern = @"^[\+]?[0-9\s]+$";
        public const string AccountNumberRegexPattern = @"^[0-9A-ZÆØÅ\-+]{1,16}$";
        public const string CountryCodeRegexPattern = @"[A-Z]{1,4}";
        public const string PhonePrefixRegexPattern = @"\+[0-9]{1,3}";
        public const string PostalCodeRegexPattern = @"[0-9]{1,16}";
    }
}