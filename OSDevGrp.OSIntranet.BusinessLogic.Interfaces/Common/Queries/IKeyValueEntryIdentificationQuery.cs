using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries
{
    public interface IKeyValueEntryIdentificationQuery : IQuery
    {
        string Key { get; set; }

        IValidator Validate(IValidator validator);
    }
}