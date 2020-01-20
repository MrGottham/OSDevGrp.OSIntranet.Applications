using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IContactCommand : IRefreshableTokenBasedCommand
    {
        IValidator Validate(IValidator validator, IContactRepository contactRepository, IAccountingRepository accountingRepository);

        IRefreshableToken ToToken();
    }
}