using System;
using System.Linq;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public class UserIdentity : ClaimsIdentity, IUserIdentity
    {
        #region Constructor

        public UserIdentity(int identifier, string externalUserIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier));

            Identifier = identifier;

            base.AddClaim(ClaimHelper.CreateExternalUserIdentifierClaim(externalUserIdentifier));
        }
        
        #endregion
        
        #region Properties

        public int Identifier { get; }

        public string ExternalUserIdentifier => Claims.Single(m => string.Compare(m.Type, ClaimHelper.ExternalUserIdentifierClaimType, StringComparison.Ordinal) == 0).Value;

        #endregion

        #region Methods

        public ClaimsIdentity ToClaimsIdentity()
        {
            return this;
        }

        #endregion
    }
}
