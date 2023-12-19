using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	public static class ClaimHelper
    {
        #region Constants

        private const string IssuerName = "CN=OSDevGrp.OSIntranet";

        public const string ExternalUserIdentifierClaimType = "urn:osdevgrp:osintranet:claims:externaluseridentifier";
        public const string FriendlyNameClaimType = "urn:osdevgrp:osintranet:claims:friendlyname";
        public const string MicrosoftTokenClaimType = "urn:osdevgrp:osintranet:claims:tokens:external:microsoft";
        public const string GoogleTokenClaimType = "urn:osdevgrp:osintranet:claims:tokens:external:google";
		public const string SecurityAdminClaimType = "urn:osdevgrp:osintranet:claims:securityadmin";
        public const string AccountingClaimType = "urn:osdevgrp:osintranet:claims:accounting";
        public const string AccountingAdministratorClaimType = "urn:osdevgrp:osintranet:claims:accounting:administrator";
        public const string AccountingCreatorClaimType = "urn:osdevgrp:osintranet:claims:accounting:creator";
        public const string AccountingModifierClaimType = "urn:osdevgrp:osintranet:claims:accounting:modifier";
        public const string AccountingViewerClaimType = "urn:osdevgrp:osintranet:claims:accounting:viewer";
        public const string MediaLibraryClaimType = "urn:osdevgrp:osintranet:claims:medialibrary";
        public const string MediaLibraryModifierClaimType = "urn:osdevgrp:osintranet:claims:medialibrary:modifier";
        public const string MediaLibraryLenderClaimType = "urn:osdevgrp:osintranet:claims:medialibrary:lender";
        public const string CommonDataClaimType = "urn:osdevgrp:osintranet:claims:commondata";
        public const string ContactsClaimType = "urn:osdevgrp:osintranet:claims:contacts";
        public const string CountryCodeClaimType = "urn:osdevgrp:osintranet:claims:countrycode";
        public const string CollectNewsClaimType = "urn:osdevgrp:osintranet:claims:collectnews";

        internal const string ClientIdClaimType = "urn:osdevgrp:osintranet:claims:clientid";

		#endregion

		#region Methods

		public static Claim CreateNameIdentifierClaim(string nameIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(nameIdentifier, nameIdentifier);

            return CreateClaim(ClaimTypes.NameIdentifier, nameIdentifier);
        }

		public static Claim CreateNameClaim(string name)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name));

            return CreateClaim(ClaimTypes.Name, name);
        }

		public static Claim CreateEmailClaim(string email)
        {
            NullGuard.NotNullOrWhiteSpace(email, email);

            return CreateClaim(ClaimTypes.Email, email);
        }

		public static Claim CreateTokenClaim(string claimType, IToken token, Func<string, string> protector)
		{
			NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType))
				.NotNull(token, nameof(token))
				.NotNull(protector, nameof(protector));

			return CreateClaim(claimType, protector(token.ToBase64String()), typeof(IToken).FullName);
		}

		public static Claim CreateTokenClaim(string claimType, IRefreshableToken refreshableToken, Func<string, string> protector)
		{
			NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType))
				.NotNull(refreshableToken, nameof(refreshableToken))
				.NotNull(protector, nameof(protector));

			return CreateClaim(claimType, protector(refreshableToken.ToBase64String()), typeof(IRefreshableToken).FullName);
		}

		public static Claim CreateSecurityAdminClaim()
        {
            return CreateClaim(SecurityAdminClaimType);
        }

		public static Claim CreateAccountingClaim(int? accountingNumber = null)
        {
            return CreateClaim(AccountingClaimType, accountingNumber?.ToString());
        }

		public static Claim CreateAccountingAdministratorClaim()
        {
            return CreateClaim(AccountingAdministratorClaimType);
        }

		public static Claim CreateAccountingCreatorClaim()
        {
            return CreateClaim(AccountingCreatorClaimType);
        }

		public static Claim CreateAccountingModifierClaim(bool canModifyAllAccountings, params int[] accountingIdentificationCollection)
        {
            NullGuard.NotNull(accountingIdentificationCollection, nameof(accountingIdentificationCollection));

            return CreateClaim(AccountingModifierClaimType, BuildClaimValue(canModifyAllAccountings, accountingIdentificationCollection));
        }

		public static Claim CreateAccountingViewerClaim(bool canAccessAllAccountings, params int[] accountingIdentificationCollection)
        {
            NullGuard.NotNull(accountingIdentificationCollection, nameof(accountingIdentificationCollection));

            return CreateClaim(AccountingViewerClaimType, BuildClaimValue(canAccessAllAccountings, accountingIdentificationCollection));
        }

		public static Claim CreateMediaLibraryClaim()
        {
            return CreateClaim(MediaLibraryClaimType);
        }

		public static Claim CreateMediaLibraryModifierClaim()
        {
	        return CreateClaim(MediaLibraryModifierClaimType);
        }

		public static Claim CreateMediaLibraryLenderClaim()
		{
			return CreateClaim(MediaLibraryLenderClaimType);
		}

		public static Claim CreateCommonDataClaim()
        {
            return CreateClaim(CommonDataClaimType);
        }

		public static Claim CreateContactsClaim()
        {
            return CreateClaim(ContactsClaimType);
        }

		public static Claim CreateCountryCodeClaim(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            return CreateClaim(CountryCodeClaimType, countryCode);
        }

		public static Claim CreateCollectNewsClaim(int? numberOfNews = null)
        {
            return CreateClaim(CollectNewsClaimType, numberOfNews?.ToString());
        }

		public static Claim CreateClaim(string type, string value = null, string valueType = null)
        {
            NullGuard.NotNullOrWhiteSpace(type, nameof(type));

            return new Claim(type, value ?? string.Empty, valueType, IssuerName);
        }

		public static Claim GetClaim(this IPrincipal principal, string type)
        {
            NullGuard.NotNull(principal, nameof(principal))
                .NotNullOrWhiteSpace(type, nameof(type));

            return GetClaim(new ClaimsPrincipal(principal), type);
        }

		public static Claim GetClaim(this ClaimsPrincipal claimsPrincipal, string type)
		{
			NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal))
				.NotNullOrWhiteSpace(type, nameof(type));

			return claimsPrincipal.FindFirst(type);
		}

		internal static Claim CreateExternalUserIdentifierClaim(string externalUserIdentifier)
		{
			NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

			return CreateClaim(ExternalUserIdentifierClaimType, externalUserIdentifier);
		}

		internal static Claim CreateFriendlyNameClaim(string friendlyName)
		{
			NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName));

			return CreateClaim(FriendlyNameClaimType, friendlyName);
		}

		internal static Claim CreateClientIdClaim(string clientId)
		{
			NullGuard.NotNullOrWhiteSpace(clientId, clientId);

			return CreateClaim(ClientIdClaimType, clientId);
		}

        private static string BuildClaimValue(bool allWildcard, params int[] collection)
        {
            NullGuard.NotNull(collection, nameof(collection));

            string claimValue = string.Join(",", collection.Select(value => value.ToString(CultureInfo.InvariantCulture)).ToArray());
            if (allWildcard && string.IsNullOrWhiteSpace(claimValue))
            {
                claimValue = "*";
            }
            else if (allWildcard)
            {
                claimValue += ",*";
            }

            return claimValue;
        }

        #endregion
    }
}