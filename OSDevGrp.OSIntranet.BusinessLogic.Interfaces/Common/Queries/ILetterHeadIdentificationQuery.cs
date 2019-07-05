using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries
{
    public interface ILetterHeadIdentificationQuery : IQuery
    {
        int Number { get; set; }

        IValidator Validate(IValidator validator, ICommonRepository commonRepository);
    }
}