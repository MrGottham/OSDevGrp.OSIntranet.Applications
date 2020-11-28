namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetAccount : IAccountBase<IBudgetAccount>
    {
        IBudgetAccountGroup BudgetAccountGroup { get; set; }

        IBudgetInfoValues ValuesForMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForYearToDateOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastYearOfStatusDate { get; }

        IBudgetInfoCollection BudgetInfoCollection { get; }
    }
}