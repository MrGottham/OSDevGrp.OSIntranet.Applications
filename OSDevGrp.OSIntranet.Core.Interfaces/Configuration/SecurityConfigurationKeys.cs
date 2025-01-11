namespace OSDevGrp.OSIntranet.Core.Interfaces.Configuration
{
    public static class SecurityConfigurationKeys
    {
	    public const string SecuritySectionName = "Security";
	    public const string JwtSectionName = "JWT";
        public const string OpenIdConnectSectionName = "OIDC";
	    public const string MicrosoftSectionName = "Microsoft";
	    public const string GoogleSectionName = "Google";
	    public const string AcmeChallengeSectionName = "AcmeChallenge";
        public const string JwtKeySectionName = "Key";

        public static readonly string JwtKeyKty = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:kty";
        public static readonly string JwtKeyN = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:n";
        public static readonly string JwtKeyE = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:e";
        public static readonly string JwtKeyD = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:d";
        public static readonly string JwtKeyDp = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:dp";
        public static readonly string JwtKeyDq = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:dq";
        public static readonly string JwtKeyP = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:p";
        public static readonly string JwtKeyQ = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:q";
        public static readonly string JwtKeyQi = $"{SecuritySectionName}:{JwtSectionName}:{JwtKeySectionName}:qi";
        public static readonly string JwtIssuer = $"{SecuritySectionName}:{JwtSectionName}:Issuer";
        public static readonly string JwtAudience = $"{SecuritySectionName}:{JwtSectionName}:Audience";

        public static readonly string OpenIdConnectAuthority = $"{SecuritySectionName}:{OpenIdConnectSectionName}:Authority";
        public static readonly string OpenIdConnectClientId = $"{SecuritySectionName}:{OpenIdConnectSectionName}:ClientId";
        public static readonly string OpenIdConnectClientSecret = $"{SecuritySectionName}:{OpenIdConnectSectionName}:ClientSecret";

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