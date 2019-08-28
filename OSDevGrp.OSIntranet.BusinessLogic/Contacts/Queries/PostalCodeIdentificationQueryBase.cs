using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public abstract class PostalCodeIdentificationQueryBase : CountryIdentificationQueryBase, IPostalCodeIdentificationQuery
    {
        #region Private variables

        private string _postalCode;
        private IPostalCode _postalCodeObject;

        #endregion

        #region Properties

        public string PostalCode
        {
            get => _postalCode;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _postalCode = value.Trim();
            }
        }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return base.Validate(validator, contactRepository)
                .ValidatePostalCode(PostalCode, GetType(), nameof(PostalCode));
        }

        protected Task<IPostalCode> GetPostalCodeAsync(IContactRepository contactRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository));

            return Task.Run(() => PostalCode.GetPostalCode(CountryCode, contactRepository, ref _postalCodeObject));
        }

        #endregion
    }
}
