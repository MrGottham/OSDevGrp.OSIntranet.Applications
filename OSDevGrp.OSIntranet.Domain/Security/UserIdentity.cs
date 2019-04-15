using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public class UserIdentity : ClaimsIdentity, IUserIdentity
    {
        #region Constructor

        public UserIdentity(int identifier, string externalUserIdentifier, IEnumerable<Claim> claims)
            : base(claims)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier))
                .NotNull(claims, nameof(claims));

            Identifier = identifier;

            base.AddClaim(ClaimHelper.CreateExternalUserIdentifierClaim(externalUserIdentifier));
        }

        #endregion
        
        #region Properties

        public int Identifier { get; }

        public string ExternalUserIdentifier => Claims.Single(m => string.Compare(m.Type, ClaimHelper.ExternalUserIdentifierClaimType, StringComparison.Ordinal) == 0).Value;

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
        }

        public void AddAuditInformations(DateTime createdUtcDateTime, string createdByIdentifier, DateTime modifiedUtcDateTime, string modifiedByIdentifier)
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
