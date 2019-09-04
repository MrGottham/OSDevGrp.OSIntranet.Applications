using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.CommandHandlers
{
    public abstract class PostalCodeIdentificationCommandBase<T> : CountryIdentificationCommandHandlerBase<T> where T : IPostalCodeIdentificationCommand
    {
        #region Constructor

        protected PostalCodeIdentificationCommandBase(IValidator validator, IContactRepository contactRepository) :
            base(validator, contactRepository)
        {
        }

        #endregion
    }
}
