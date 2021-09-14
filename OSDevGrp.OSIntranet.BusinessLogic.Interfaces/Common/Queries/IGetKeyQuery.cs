using System.Collections.Generic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries
{
    public interface IGetKeyQuery : IQuery
    {
        IEnumerable<string> KeyElementCollection { get; set; }

        IValidator Validate(IValidator validator);
    }
}