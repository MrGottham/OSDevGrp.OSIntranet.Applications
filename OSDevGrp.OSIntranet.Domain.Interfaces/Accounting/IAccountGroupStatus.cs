namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountGroupStatus : IAccountGroup, ICalculable<IAccountGroupStatus>
    {
        IAccountCollection AccountCollection { get; }

        IAccountCollectionValues ValuesAtStatusDate { get; }

        IAccountCollectionValues ValuesAtEndOfLastMonthFromStatusDate { get; }

        IAccountCollectionValues ValuesAtEndOfLastYearFromStatusDate { get; }
    }
}