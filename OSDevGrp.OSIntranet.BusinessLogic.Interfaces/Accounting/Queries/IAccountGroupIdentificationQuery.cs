using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries
{
    public interface IAccountGroupIdentificationQuery : IQuery
    {
        int Number { get; set; }
    }
}