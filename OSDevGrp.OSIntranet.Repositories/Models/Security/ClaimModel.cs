using System.Security.Claims;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClaimModel
    {
        public virtual int ClaimIdentifier { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }
    }

    internal static class ClaimModelExtensions
    {
        internal static Claim ToDomain(this ClaimModel claimModel)
        {
            return ClaimHelper.CreateClaim(claimModel.ClaimType, claimModel.ClaimValue);
        }
    }
}
