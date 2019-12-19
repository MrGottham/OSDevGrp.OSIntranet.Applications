using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class DeleteContactGroupCommand : ContactGroupIdentificationCommandBase, IDeleteContactGroupCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return base.Validate(validator, contactRepository)
                .Object.ShouldBeKnownValue(Number, number => Task.Run(async () => await GetContactGroupAsync(contactRepository) != null), GetType(), nameof(Number))
                .Object.ShouldBeDeletable(Number, number => GetContactGroupAsync(contactRepository), GetType(), nameof(Number));
        }

        #endregion
    }
}
