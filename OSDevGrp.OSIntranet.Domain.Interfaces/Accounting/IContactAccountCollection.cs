using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IContactAccountCollection : IAccountCollectionBase<IContactAccount, IContactAccountCollection>
    {
        IContactAccountCollectionValues ValuesAtStatusDate { get; }

        IContactAccountCollectionValues ValuesAtEndOfLastMonthFromStatusDate { get; }

        IContactAccountCollectionValues ValuesAtEndOfLastYearFromStatusDate { get; }

        Task<IContactAccountCollection> FindDebtorsAsync();

        Task<IContactAccountCollection> FindCreditorsAsync();
    }
}