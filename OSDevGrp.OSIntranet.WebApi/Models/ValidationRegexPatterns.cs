namespace OSDevGrp.OSIntranet.WebApi.Models
{
    public static class ValidationRegexPatterns
    {
        public const string MailAddressRegexPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public const string PhoneNumberRegexPattern = @"^[\+]?[0-9\s]+$";
        public const string AccountNumberRegexPattern = @"[0-9A-Z\-+]{1,16}";
    }
}