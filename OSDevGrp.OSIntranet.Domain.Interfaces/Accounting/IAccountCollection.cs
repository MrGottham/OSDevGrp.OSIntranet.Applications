using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountCollection : IAccountCollectionBase<IAccount, IAccountCollection>
    {
        IAccountCollectionValues ValuesAtStatusDate { get; }

        IAccountCollectionValues ValuesAtEndOfLastMonthFromStatusDate { get; }

        IAccountCollectionValues ValuesAtEndOfLastYearFromStatusDate { get; }

        Task<IEnumerable<IAccountGroupStatus>> GroupByAccountGroupAsync();
    }
}