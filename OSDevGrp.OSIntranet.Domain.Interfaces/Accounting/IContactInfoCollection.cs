namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IContactInfoCollection : IInfoCollection<IContactInfo, IContactInfoCollection>
    {
        IContactInfoValues ValuesAtStatusDate { get; }

        IContactInfoValues ValuesAtEndOfLastMonthFromStatusDate { get; }

        IContactInfoValues ValuesAtEndOfLastYearFromStatusDate { get; }
    }
}