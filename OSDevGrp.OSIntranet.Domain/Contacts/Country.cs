using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class Country : AuditableBase, ICountry
    {
        #region Private variables

        private string _code;
        private string _name;
        private string _universalName;
        private string _phonePrefix;

        #endregion

        #region Constructor

        public Country(string code, string name, string universalName, string phonePrefix)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code))
                .NotNullOrWhiteSpace(name, nameof(name))
                .NotNullOrWhiteSpace(universalName, nameof(universalName))
                .NotNullOrWhiteSpace(phonePrefix, nameof(phonePrefix));

            Code = code;
            Name = name;
            UniversalName = universalName;
            PhonePrefix = phonePrefix;
        }

        #endregion

        #region Properties

        public string Code
        {
            get => _code;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _code = value.Trim().ToUpper();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _name = value.Trim();
            }
        }

        public string UniversalName
        {
            get => _universalName;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _universalName = value.Trim();
            }
        }

        public string PhonePrefix
        {
            get => _phonePrefix;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _phonePrefix = value.Trim();
            }
        }

        public bool Deletable { get; private set; }

        public bool DefaultForPrincipal { get; private set; }

        #endregion

        #region Methods

        public void AllowDeletion()
        {
            Deletable = true;
        }

        public void DisallowDeletion()
        {
            Deletable = false;
        }

        public void ApplyDefaultForPrincipal(string defaultCountryCode)
        {
            DefaultForPrincipal = string.IsNullOrWhiteSpace(defaultCountryCode) == false && string.CompareOrdinal(Code, defaultCountryCode) == 0;
        }

        #endregion
    }
}
