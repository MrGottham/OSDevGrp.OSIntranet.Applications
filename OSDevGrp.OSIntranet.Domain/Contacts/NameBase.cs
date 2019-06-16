using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public abstract class NameBase : IName
    {
        #region Properties

        public abstract string DisplayName { get; }

        #endregion

        #region Methods

        public abstract void SetName(string fullName);

        #endregion
    }
}
