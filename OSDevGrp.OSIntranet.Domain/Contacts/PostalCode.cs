using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class PostalCode : AuditableBase, IPostalCode
    {
        #region Private variables

        private string _code;
        private string _city;
        private string _state;

        #endregion

        #region Constructors

        public PostalCode(ICountry country, string code, string city) 
            : this(country, code, city, null)
        {
        }

        public PostalCode(ICountry country, string code, string city, string state)
        {
            NullGuard.NotNull(country, nameof(country))
                .NotNullOrWhiteSpace(code, nameof(code))
                .NotNullOrWhiteSpace(city, nameof(city));

            Country = country;
            Code = code;
            City = city;
            State = state;
        }

        #endregion

        #region Properties

        public ICountry Country { get; }

        public string Code
        {
            get => _code;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _code = value.Trim();
            }
        }

        public string City
        {
            get => _city;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _city = value.Trim();
            }
        }

        public string State
        {
            get => _state;
            set => _state = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public bool Deletable { get; private set; }

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

        #endregion
    }
}
