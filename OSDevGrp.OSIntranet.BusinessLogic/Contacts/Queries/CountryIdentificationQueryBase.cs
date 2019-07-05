using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public abstract class CountryIdentificationQueryBase : ICountryIdentificationQuery
    {
        #region Private variables

        private string _countryCode;
        private ICountry _country;

        #endregion

        #region Properties

        public string CountryCode
        {
            get => _countryCode;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _countryCode = value.Trim().ToUpper();
            }
        }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return validator.ValidateCountryCode(CountryCode, GetType(), nameof(CountryCode));
        }

        protected Task<ICountry> GetCountry(IContactRepository contactRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository));

            return Task.Run(() => CountryCode.GetCountry(contactRepository, ref _country));
        }

        #endregion
    }
}
