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

        #endregion

        #region Methods

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

        internal static Claim CreateClaim(string type, string value = null, string valueType = null)
        {
            NullGuard.NotNullOrWhiteSpace(type, nameof(type));

            return new Claim(type, value, valueType, IssuerName);
        }

        #endregion
    }
}
