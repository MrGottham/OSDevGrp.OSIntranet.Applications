namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface ICreditInfoCollection : IInfoCollection<ICreditInfo, ICreditInfoCollection>
    {
        ICreditInfoValues ValuesAtStatusDate { get; }

        ICreditInfoValues ValuesAtEndOfLastMonthFromStatusDate { get; }

        ICreditInfoValues ValuesAtEndOfLastYearFromStatusDate { get; }
    }
}