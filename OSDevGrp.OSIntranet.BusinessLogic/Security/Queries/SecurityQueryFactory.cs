using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
    public static class SecurityQueryFactory
	{
		#region Methods

		public static IGetMicrosoftTokenQuery BuildGetMicrosoftTokenQuery(Func<string, string> unprotect)
		{
			return new GetMicrosoftTokenQuery(unprotect);
		}

		public static IGetMicrosoftTokenQuery BuildGetMicrosoftTokenQuery(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
		{
			return new GetMicrosoftTokenQuery(claimsPrincipal, unprotect);
		}

		public static IGetGoogleTokenQuery BuildGetGoogleTokenQuery(Func<string, string> unprotect)
		{
			return new GetGoogleTokenQuery(unprotect);
		}

		public static IGetGoogleTokenQuery BuildGetGoogleTokenQuery(ClaimsPrincipal claimsPrincipal, Func<string, string> unprotect)
		{
			return new GetGoogleTokenQuery(claimsPrincipal, unprotect);
		}

        public static IGetJsonWebKeySetQuery BuildGetJsonWebKeySetQuery()
        {
            return new GetJsonWebKeySetQuery();
        }

        public static IGetOpenIdProviderConfigurationQuery BuildGetOpenIdProviderConfigurationQuery(Uri authorizationEndpoint, Uri tokenEndpoint, Uri jsonWebKeySetEndpoint, Uri userInfoEndpoint = null, Uri registrationEndpoint = null, Uri serviceDocumentationEndpoint = null, Uri registrationPolicyEndpoint = null, Uri registrationTermsOfServiceEndpoint = null)
        {
            return new GetOpenIdProviderConfigurationQuery(authorizationEndpoint, tokenEndpoint, jsonWebKeySetEndpoint, userInfoEndpoint, registrationEndpoint, serviceDocumentationEndpoint, registrationPolicyEndpoint, registrationTermsOfServiceEndpoint);
        }

		#endregion
    }
}