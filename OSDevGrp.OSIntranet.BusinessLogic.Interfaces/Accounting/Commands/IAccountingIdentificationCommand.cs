using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IAccountingIdentificationCommand : ICommand
    {
        int AccountingNumber { get; set; }

        IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository);
    }
}