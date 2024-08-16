using OSDevGrp.OSIntranet.Core;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class ClaimExtensions
    {
        #region Methods

        public static Claim[] Concat(this Claim[] claims, params Claim[] extraClaims)
        {
            NullGuard.NotNull(claims, nameof(claims))
                .NotNull(extraClaims, nameof(extraClaims));

            return claims.Concat(extraClaims.AsEnumerable()).ToArray();
        }

        #endregion
    }
}