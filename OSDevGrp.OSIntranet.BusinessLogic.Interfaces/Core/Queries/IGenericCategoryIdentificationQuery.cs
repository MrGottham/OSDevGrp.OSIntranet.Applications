using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Queries
{
    public interface IGenericCategoryIdentificationQuery : IQuery
    {
        int Number { get; }

        IValidator Validate(IValidator validator);
    }
}