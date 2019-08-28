using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public abstract class PostalCodeIdentificationQueryBase : CountryIdentificationQueryBase, IPostalCodeIdentificationQuery
    {
        #region Private variables

        private string _postalCode;

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

        #endregion
    }
}
