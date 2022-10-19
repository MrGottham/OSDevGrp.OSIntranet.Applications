using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetAccountGroup : IAccountGroupBase
    {
        Task<IBudgetAccountGroupStatus> CalculateAsync(DateTime statusDate, IBudgetAccountCollection budgetAccountCollection);
    }
}