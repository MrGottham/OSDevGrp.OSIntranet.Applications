using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public static class ClaimHelper
    {
        #region Constants

        public const string IssuerName = "CN=OSDevGrp.OSIntranet";

        public const string ExternalUserIdentifierClaimType = "urn:osdevgrp:osintranet:claims:externaluseridentifier";
        public const string FriendlyNameClaimType = "urn:osdevgrp:osintranet:claims:friendlyname";
        public const string ClientIdClaimType = "urn:osdevgrp:osintranet:claims:clientid";
        public const string SecurityAdminClaimType = "urn:osdevgrp:osintranet:claims:securityadmin";
        public const string AccountingClaimType = "urn:osdevgrp:osintranet:claims:accounting";
        public const string CommonDataClaimType = "urn:osdevgrp:osintranet:claims:commondata";
        public const string ContactsClaimType = "urn:osdevgrp:osintranet:claims:contacts";
        public const string CountryCodeClaimType = "urn:osdevgrp:osintranet:claims:countrycode";

        #endregion

        #region Methods

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

        public static Claim CreateClaim(string type, string value = null, string valueType = null)
        {
            NullGuard.NotNullOrWhiteSpace(type, nameof(type));

            return new Claim(type, value ?? string.Empty, valueType, IssuerName);
        }

        #endregion
    }
}
