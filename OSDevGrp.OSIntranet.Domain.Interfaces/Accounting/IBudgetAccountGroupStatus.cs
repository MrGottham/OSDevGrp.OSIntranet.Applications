namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IBudgetAccountGroupStatus : IBudgetAccountGroup, ICalculable<IBudgetAccountGroupStatus>
    {
        IBudgetAccountCollection BudgetAccountCollection { get; }

        IBudgetInfoValues ValuesForMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastMonthOfStatusDate { get; }

        IBudgetInfoValues ValuesForYearToDateOfStatusDate { get; }

        IBudgetInfoValues ValuesForLastYearOfStatusDate { get; }
    }
}