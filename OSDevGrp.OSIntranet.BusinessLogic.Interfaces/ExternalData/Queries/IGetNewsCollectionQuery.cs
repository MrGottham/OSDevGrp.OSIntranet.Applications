using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.ExternalData.Queries
{
    public interface IGetNewsCollectionQuery : IQuery
    {
        bool FromExternalDashboard { get; }

        int NumberOfNews { get; }

        IValidator Validate(IValidator validator);
    }
}