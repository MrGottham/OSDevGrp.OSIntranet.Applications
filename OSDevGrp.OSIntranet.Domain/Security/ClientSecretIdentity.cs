using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	internal class ClientSecretIdentity : ClaimsIdentity, IClientSecretIdentity
    {
        #region Constructor

        internal ClientSecretIdentity(int identifier, string friendlyName, string clientId, string clientSecret, IEnumerable<Claim> claims) 
            : base(claims)
        {
            NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret))
                .NotNull(claims, nameof(claims));

            Identifier = identifier;
            ClientSecret = clientSecret;

            base.AddClaim(ClaimHelper.CreateNameClaim(friendlyName));
            base.AddClaim(ClaimHelper.CreateNameIdentifierClaim(clientId));
            base.AddClaim(ClaimHelper.CreateFriendlyNameClaim(friendlyName));
            base.AddClaim(ClaimHelper.CreateClientIdClaim(clientId));
        }

        #endregion

        #region Properties

        public int Identifier { get; }

        public string FriendlyName => Claims.Single(m => string.Compare(m.Type, ClaimHelper.FriendlyNameClaimType, StringComparison.Ordinal) == 0).Value;

        public string ClientId => Claims.Single(m => string.Compare(m.Type, ClaimHelper.ClientIdClaimType, StringComparison.Ordinal) == 0).Value;

        public string ClientSecret { get; private set; }

        public DateTime CreatedDateTime { get; private set; }

        public string CreatedByIdentifier { get; private set; }

        public DateTime ModifiedDateTime { get; private set; }

        public string ModifiedByIdentifier { get; private set; }

        #endregion

        #region Methods

        public ClaimsIdentity ToClaimsIdentity()
        {
            return this;
        }

        public void ClearSensitiveData()
        {
            ClientSecret = null;
        }

        public void AddAuditInformation(DateTime createdUtcDateTime, string createdByIdentifier, DateTime modifiedUtcDateTime, string modifiedByIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(createdByIdentifier, nameof(createdByIdentifier))
                .NotNullOrWhiteSpace(modifiedByIdentifier, nameof(modifiedByIdentifier));

            CreatedDateTime = createdUtcDateTime.ToLocalTime();
            CreatedByIdentifier = createdByIdentifier;
            ModifiedDateTime = modifiedUtcDateTime.ToLocalTime();
            ModifiedByIdentifier = modifiedByIdentifier;
        }

        #endregion
    }
}