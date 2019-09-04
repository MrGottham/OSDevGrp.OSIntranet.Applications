using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class CreatePostalCodeCommand : PostalCodeCommandBase, ICreatePostalCodeCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IContactRepository contactRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(contactRepository, nameof(contactRepository));

            return base.Validate(validator, contactRepository)
                .Object.ShouldBeUnknownValue(PostalCode, postalCode => Task.Run(async () => await GetPostalCodeAsync(contactRepository) == null), GetType(), nameof(PostalCode));
        }

        #endregion
    }
}
