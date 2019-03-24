namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class UserIdentityClaimModel : IdentityClaimModelBase
    {
        public virtual int UserIdentityClaimIdentifier { get; set; }

        public virtual int UserIdentityIdentifier { get; set; }

        public virtual UserIdentityModel UserIdentity { get; set; }
    }
}
