namespace OSDevGrp.OSIntranet.Core.Interfaces.Configuration
{
    public static class SecurityConfigurationKeys
    {
	    public const string SecuritySectionName = "Security";
	    public const string JwtSectionName = "JWT";
	    public const string MicrosoftSectionName = "Microsoft";
	    public const string GoogleSectionName = "Google";
	    public const string AcmeChallengeSectionName = "AcmeChallenge";

		public static readonly string JwtKey = $"{SecuritySectionName}:{JwtSectionName}:Key";

        public static readonly string MicrosoftClientId = $"{SecuritySectionName}:{MicrosoftSectionName}:ClientId";
        public static readonly string MicrosoftClientSecret = $"{SecuritySectionName}:{MicrosoftSectionName}:ClientSecret";
        public static readonly string MicrosoftTenant = $"{SecuritySectionName}:{MicrosoftSectionName}:Tenant";

        public static readonly string GoogleClientId = $"{SecuritySectionName}:{GoogleSectionName}:ClientId";
        public static readonly string GoogleClientSecret = $"{SecuritySectionName}:{GoogleSectionName}:ClientSecret";

        public static readonly string TrustedDomainCollection = $"{SecuritySectionName}:TrustedDomainCollection";

        public static readonly string AcmeChallengeWellKnownChallengeToken = $"{SecuritySectionName}:{AcmeChallengeSectionName}:WellKnownChallengeToken";
        public static readonly string AcmeChallengeConstructedKeyAuthorization = $"{SecuritySectionName}:{AcmeChallengeSectionName}:ConstructedKeyAuthorization";
    }
}