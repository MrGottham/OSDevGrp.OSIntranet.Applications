using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class Company : ICompany
    {
        #region Private variables

        private string _primaryPhone;
        private string _secondaryPhone;
        private string _homePage;

        #endregion

        #region Constructors

        public Company(ICompanyName name)
            : this(name, new Address())
        {
        }

        public Company(ICompanyName name, IAddress address)
        {
            NullGuard.NotNull(name, nameof(name))
                .NotNull(address, nameof(address));

            Name = name;
            Address = address;
        }

        #endregion

        #region Properties

        public ICompanyName Name { get; }

        public IAddress Address { get; }

        public string PrimaryPhone
        {
            get => _primaryPhone;
            set => _primaryPhone = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string SecondaryPhone
        {
            get => _secondaryPhone;
            set => _secondaryPhone = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string HomePage
        {
            get => _homePage;
            set => _homePage = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        #endregion
    }
}
