using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IExternalDashboardRepository : IRepository
    {
        Task<IEnumerable<INews>> GetNewsAsync(int numberOfNews);
    }
}