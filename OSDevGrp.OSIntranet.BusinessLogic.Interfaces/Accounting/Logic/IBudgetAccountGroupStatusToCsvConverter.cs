using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic
{
    public interface IBudgetAccountGroupStatusToCsvConverter : IDomainObjectToCsvConverter<IBudgetAccountGroupStatus>
    {
    }
}