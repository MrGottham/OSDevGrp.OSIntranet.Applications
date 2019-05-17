using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries
{
    public interface ILetterHeadIdentificationQuery : IQuery
    {
        int Number { get; set; }
    }
}