using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class IncomeStatementDisplayer : IIncomeStatementDisplayer
{
    #region Constructor

    private IncomeStatementDisplayer(string incomeStatementLabel, string monthOfStatusDateLabel, string lastMonthOfStatusDateLabel, string yearToDateOfStatusDateLabel, string lastYearOfStatusDateLabel, string budgetLabel, string postedLabel, string availableLabel, IValueDisplayer statusDate, IReadOnlyCollection<IIncomeStatementLineDisplayer> lines)
    {
        IncomeStatementLabel = incomeStatementLabel;
        MonthOfStatusDateLabel = monthOfStatusDateLabel;
        LastMonthOfStatusDateLabel = lastMonthOfStatusDateLabel;
        YearToDateOfStatusDateLabel = yearToDateOfStatusDateLabel;
        LastYearOfStatusDateLabel = lastYearOfStatusDateLabel;
        BudgetLabel = budgetLabel;
        PostedLabel = postedLabel;
        AvailableLabel = availableLabel;
        StatusDate = statusDate;
        Lines = lines;
    }

    #endregion

    #region Properties

    public string IncomeStatementLabel { get; }

    public string MonthOfStatusDateLabel { get; }

    public string LastMonthOfStatusDateLabel { get; }

    public string YearToDateOfStatusDateLabel { get; }

    public string LastYearOfStatusDateLabel { get; }

    public string BudgetLabel { get; }

    public string PostedLabel { get; }

    public string AvailableLabel { get; }

    public IValueDisplayer StatusDate { get; }

    public IReadOnlyCollection<IIncomeStatementLineDisplayer> Lines { get; }

    #endregion

    #region Methods

    internal static async Task<IncomeStatementDisplayer> CreateAsync(StaticTextKey budget, StaticTextKey posted, StaticTextKey available, IStaticTextProvider staticTextProvider, AccountingModel accounting, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string incomeStatementLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.IncomeStatement, StaticTextKey.IncomeStatement.DefaultArguments(), formatProvider, cancellationToken);
        string monthOfStatusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BudgetStatementForMonthOfStatusDate, StaticTextKey.BudgetStatementForMonthOfStatusDate.DefaultArguments(), formatProvider, cancellationToken);
        string lastMonthOfStatusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BudgetStatementForLastMonthOfStatusDate, StaticTextKey.BudgetStatementForLastMonthOfStatusDate.DefaultArguments(), formatProvider, cancellationToken);
        string yearToDateOfStatusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BudgetStatementForYearToDateOfStatusDate, StaticTextKey.BudgetStatementForYearToDateOfStatusDate.DefaultArguments(), formatProvider, cancellationToken);
        string lastYearOfStatusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BudgetStatementForLastYearOfStatusDate, StaticTextKey.BudgetStatementForLastYearOfStatusDate.DefaultArguments(), formatProvider, cancellationToken);
        string budgetLabel = await staticTextProvider.GetStaticTextAsync(budget, budget.DefaultArguments(), formatProvider, cancellationToken);
        string postedLabel = await staticTextProvider.GetStaticTextAsync(posted, posted.DefaultArguments(), formatProvider, cancellationToken);
        string availableLabel = await staticTextProvider.GetStaticTextAsync(available, available.DefaultArguments(), formatProvider, cancellationToken);

        string statusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.StatusDate, StaticTextKey.StatusDate.DefaultArguments(), formatProvider, cancellationToken);
        IValueDisplayer statusDate = new ValueDisplayer<DateTimeOffset>(statusDateLabel, accounting.StatusDate, formatProvider, (value, fp) => value.ToString("D", fp));

        IncomeStatementLineDisplayer[] lines = accounting.BudgetAccounts
            .GroupBy(budgetAccount => budgetAccount.BudgetAccountGroup.Number)
            .OrderBy(group => group.Key)
            .Select(group => IncomeStatementLineDisplayer.Create(group.Key.ToString(formatProvider), group.First().BudgetAccountGroup.Name, group, formatProvider))
            .ToArray();

        string totalLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.IncomeStatementTotal, StaticTextKey.IncomeStatementTotal.DefaultArguments(), formatProvider, cancellationToken);
        IncomeStatementLineDisplayer totalLine = IncomeStatementLineDisplayer.Create(null, totalLabel, accounting.BudgetAccounts, formatProvider);

        return new IncomeStatementDisplayer(
            incomeStatementLabel,
            monthOfStatusDateLabel,
            lastMonthOfStatusDateLabel,
            yearToDateOfStatusDateLabel,
            lastYearOfStatusDateLabel,
            budgetLabel,
            postedLabel,
            availableLabel,
            statusDate,
            lines.Concat([totalLine]).ToArray());
    }

    #endregion
}