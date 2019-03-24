namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class ClientSecretIdentityClaimModel : IdentityClaimModelBase
    {
        public virtual int ClientSecretIdentityClaimIdentifier { get; set; }

        public virtual int ClientSecretIdentityIdentifier { get; set; }

        public virtual ClientSecretIdentityModel ClientSecretIdentity { get; set; }
    }
}
