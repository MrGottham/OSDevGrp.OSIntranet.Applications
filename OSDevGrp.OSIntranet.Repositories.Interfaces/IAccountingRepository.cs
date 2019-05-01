using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IAccountingRepository : IRepository
    {
        Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync();

        Task<IAccountGroup> GetAccountGroupAsync(int number);

        Task<IAccountGroup> CreateAccountGroupAsync(IAccountGroup accountGroup);

        Task<IAccountGroup> UpdateAccountGroupAsync(IAccountGroup accountGroup);

        Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync();

        Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(int number);

        Task<IBudgetAccountGroup> CreateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup);

        Task<IBudgetAccountGroup> UpdateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup);
    }
}