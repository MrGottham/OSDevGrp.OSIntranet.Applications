using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
    public abstract class IdentityIdentificationQueryBase : IIdentityIdentificationQueryBase
    {
        #region Properties

        public int IdentityIdentifier { get; set; }

        #endregion
    }
}
