using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.QueryBus
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<in TQuery,  TResult> : IQueryHandler where TQuery : IQuery
    {
        Task<TResult> QueryAsync(TQuery query);
    }
}