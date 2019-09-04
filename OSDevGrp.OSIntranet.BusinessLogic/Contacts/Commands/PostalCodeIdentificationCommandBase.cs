using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public abstract class PostalCodeIdentificationCommandBase : CountryIdentificationCommandBase, IPostalCodeIdentificationCommand
    {
        #region Private variables

        private IPostalCode _postalCode;

        #endregion

        #region Properties

        public string PostalCode { get; set; }

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

            return Task.Run(() => PostalCode.GetPostalCode(CountryCode, contactRepository, ref _postalCode));
        }

        #endregion
    }
}
