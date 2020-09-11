using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries
{
    public interface IExportContactCollectionQuery : IRefreshableTokenBasedQuery, IExportQuery
    {
        IGetContactCollectionQuery ToGetContactCollectionQuery();
    }
}