namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetInfoCollection : IInfoCollection<IBudgetInfo, IBudgetInfoCollection>
    {
        IBudgetInfoValues ValuesForMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForYearToDateOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastYearOfStatusDate { get; }
    }
}