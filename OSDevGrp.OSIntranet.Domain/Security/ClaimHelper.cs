using System;
using System.Security.Claims;
using System.Security.Principal;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public static class ClaimHelper
    {
        #region Constants

        public const string IssuerName = "CN=OSDevGrp.OSIntranet";

        public const string ExternalUserIdentifierClaimType = "urn:osdevgrp:osintranet:claims:externaluseridentifier";
        public const string FriendlyNameClaimType = "urn:osdevgrp:osintranet:claims:friendlyname";
        public const string ClientIdClaimType = "urn:osdevgrp:osintranet:claims:clientid";
        public const string TokenClaimType = "urn:osdevgrp:osintranet:claims:token";
        public const string SecurityAdminClaimType = "urn:osdevgrp:osintranet:claims:securityadmin";
        public const string AccountingClaimType = "urn:osdevgrp:osintranet:claims:accounting";
        public const string CommonDataClaimType = "urn:osdevgrp:osintranet:claims:commondata";
        public const string ContactsClaimType = "urn:osdevgrp:osintranet:claims:contacts";
        public const string CountryCodeClaimType = "urn:osdevgrp:osintranet:claims:countrycode";
        public const string CollectNewsClaimType = "urn:osdevgrp:osintranet:claims:collectnews";

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

        public static Claim CreateExternalUserIdentifierClaim(string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            return CreateClaim(ExternalUserIdentifierClaimType, externalUserIdentifier);
        }

        public static Claim CreateFriendlyNameClaim(string friendlyName)
        {
            NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName));

            return CreateClaim(FriendlyNameClaimType, friendlyName);
        }

        public static Claim CreateClientIdClaim(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, clientId);

            return CreateClaim(ClientIdClaimType, clientId);
        }

        public static Claim CreateTokenClaim(IToken token, Func<string, string> protect)
        {
            NullGuard.NotNull(token, nameof(token))
                .NotNull(protect, nameof(protect));

            return CreateClaim(TokenClaimType, protect(token.ToBase64()));
        }

        public static Claim CreateSecurityAdminClaim()
        {
            return CreateClaim(SecurityAdminClaimType);
        }

        public static Claim CreateAccountingClaim(int? accountingNumber = null)
        {
            return CreateClaim(AccountingClaimType, accountingNumber?.ToString());
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

        #endregion
    }
}