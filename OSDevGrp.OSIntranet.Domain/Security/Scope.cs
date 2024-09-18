using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class Scope : IScope
    {
        #region Constructor

        public Scope(string name, string description, IEnumerable<string> relatedClaims)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNullOrWhiteSpace(description, nameof(description))
                .NotNull(relatedClaims, nameof(relatedClaims));

            Name = name.Trim();
            Description = description.Trim();
            RelatedClaims = new HashSet<string>(relatedClaims);
        }

        #endregion

        #region Properties

        public string Name { get; }

        public string Description { get; }

        public IEnumerable<string> RelatedClaims { get; }

        public IEnumerable<Claim> Filter(IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(claims, nameof(claims));

            return claims.Where(claim => string.IsNullOrWhiteSpace(claim.Type) == false && RelatedClaims.Any(relatedClaim => string.IsNullOrWhiteSpace(relatedClaim) == false && claim.Type == relatedClaim)).ToArray();
        }

        #endregion
    }
}