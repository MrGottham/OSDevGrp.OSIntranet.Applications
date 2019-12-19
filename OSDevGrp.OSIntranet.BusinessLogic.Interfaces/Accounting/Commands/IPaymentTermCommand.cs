using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IPaymentTermCommand : IPaymentTermIdentificationCommand
    {
        string Name { get; set; }

        IPaymentTerm ToDomain();
    }
}
