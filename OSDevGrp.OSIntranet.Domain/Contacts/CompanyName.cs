using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class CompanyName : NameBase, ICompanyName
    {
        #region Private variables

        private string _fullName;

        #endregion

        #region Constructor

        public CompanyName(string fullName)
        {
            NullGuard.NotNullOrWhiteSpace(fullName, nameof(fullName));

            FullName = fullName;
        }

        #endregion

        #region Properties

        public string FullName
        {
            get => _fullName;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _fullName = value.Trim();
            }
        }

        public override string DisplayName => FullName;

        #endregion

        #region Methods

        public override void SetName(string fullName)
        {
            NullGuard.NotNullOrWhiteSpace(fullName, nameof(fullName));

            FullName = fullName;
        }

        #endregion
    }
}
