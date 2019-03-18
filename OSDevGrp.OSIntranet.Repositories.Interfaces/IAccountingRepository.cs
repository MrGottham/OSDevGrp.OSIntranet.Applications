using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IAccountingRepository : IRepository
    {
        Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync();

        Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync();
    }
}