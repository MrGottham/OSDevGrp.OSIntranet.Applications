using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetAccountCollection : IAccountCollectionBase<IBudgetAccount, IBudgetAccountCollection>
    {
        IBudgetInfoValues ValuesForMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForYearToDateOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastYearOfStatusDate { get; }

        Task<IEnumerable<IBudgetAccountGroupStatus>> GroupByBudgetAccountGroupAsync();
    }
}