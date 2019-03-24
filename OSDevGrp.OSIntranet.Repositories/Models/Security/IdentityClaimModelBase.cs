using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal abstract class IdentityClaimModelBase
    {
        public virtual int ClaimIdentifier { get; set; }

        public virtual ClaimModel Claim { get; set; }

        public virtual string ClaimValue { get; set; }
    }

    internal static class IdentityClaimModelBaseExtensions
    {
        internal static Claim ToDomain(this IdentityClaimModelBase identityClaimModel)
        {
            NullGuard.NotNull(identityClaimModel, nameof(identityClaimModel));

            return string.IsNullOrWhiteSpace(identityClaimModel.ClaimValue)
                ? identityClaimModel.Claim.ToDomain()
                : ClaimHelper.CreateClaim(identityClaimModel.Claim.ClaimType, identityClaimModel.ClaimValue);
        }
    }
}
