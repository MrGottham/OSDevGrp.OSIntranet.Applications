using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IPaymentTermIdentificationCommand : ICommand
    {
        int Number { get; set; }

        IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository);
    }
}