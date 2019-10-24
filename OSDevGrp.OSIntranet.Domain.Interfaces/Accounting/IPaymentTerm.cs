using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPaymentTerm : IAuditable, IDeletable
    {
        int Number { get; }

        string Name { get; }
    }
}
