using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries
{
    public interface IAccountGroupIdentificationQueryBase : IQuery
    {
        int Number { get; set; }
    }
}