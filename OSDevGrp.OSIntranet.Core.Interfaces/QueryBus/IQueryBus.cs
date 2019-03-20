using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.QueryBus
{
    public interface IQueryBus
    {
        Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery;
    }
}