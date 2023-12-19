namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
	internal static class AuthenticationSessionKeys
	{
		internal const string ExternalTokenClaimTypeKey = ".Claim.external_claim_type";
		internal const string TokenTypeKey = ".Token.token_type";
		internal const string AccessTokenKey = ".Token.access_token";
		internal const string RefreshTokenKey = ".Token.refresh_token";
		internal const string ExpiresInKey = ".Token.expires_in";
		internal const string ExpiresAtKey = ".Token.expires_at";
	}
}