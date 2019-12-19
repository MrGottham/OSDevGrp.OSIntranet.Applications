using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public abstract class ContactGroupIdentificationCommandBase : IContactGroupIdentificationCommand
    {
        #region Private variables

        private IContactGroup _contactGroup;

        #endregion

        #region Properties

        public int Number { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return validator.ValidateContactGroupIdentifier(Number, GetType(), nameof(Number));
        }

        protected Task<IContactGroup> GetContactGroupAsync(IContactRepository contactRepository)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository));

            return Task.Run(() => Number.GetContactGroup(contactRepository, ref _contactGroup));
        }

        #endregion
    }
}
