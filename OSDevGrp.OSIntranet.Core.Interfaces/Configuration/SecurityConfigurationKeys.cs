namespace OSDevGrp.OSIntranet.Core.Interfaces.Configuration
{
    public static class SecurityConfigurationKeys
    {
        public const string JwtKey = "Security:JWT:Key";

        public const string MicrosoftClientId = "Security:Microsoft:ClientId";
        public const string MicrosoftClientSecret = "Security:Microsoft:ClientSecret";
        public const string MicrosoftTenant = "Security:Microsoft:Tenant";

        public const string GoogleClientId = "Security:Google:ClientId";
        public const string GoogleClientSecret = "Security:Google:ClientSecret";

        public const string TrustedDomainCollection = "Security:TrustedDomainCollection";

        public const string AcmeChallengeWellKnownChallengeToken = "Security:AcmeChallenge:WellKnownChallengeToken";
        public const string AcmeChallengeConstructedKeyAuthorization = "Security:AcmeChallenge:ConstructedKeyAuthorization";
    }
}