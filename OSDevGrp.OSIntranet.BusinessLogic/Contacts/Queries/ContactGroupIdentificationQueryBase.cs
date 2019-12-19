using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public abstract class ContactGroupIdentificationQueryBase : IContactGroupIdentificationQuery
    {
        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return validator.ValidateContactGroupIdentifier(Number, GetType(), nameof(Number));
        }

        #endregion
    }
}
