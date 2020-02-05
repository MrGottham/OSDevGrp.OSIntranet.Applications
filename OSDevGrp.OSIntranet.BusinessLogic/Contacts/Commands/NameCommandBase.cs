using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public abstract class NameCommandBase : INameCommand
    {
        #region Methods

        public abstract IValidator Validate(IValidator validator);

        public abstract IName ToDomain();

        #endregion
    }
}