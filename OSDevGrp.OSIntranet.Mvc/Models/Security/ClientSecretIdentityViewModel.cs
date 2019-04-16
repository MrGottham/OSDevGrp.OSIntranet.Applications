namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    public class ClientSecretIdentityViewModel : IdentityViewModelBase
    {
        #region Properties

        public string FriendlyName { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        #endregion
    }
}
