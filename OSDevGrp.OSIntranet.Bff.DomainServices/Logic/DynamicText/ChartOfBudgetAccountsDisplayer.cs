using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfBudgetAccountsDisplayer : IChartOfBudgetAccountsDisplayer
{
	#region Constructor

	private ChartOfBudgetAccountsDisplayer(string chartOfBudgetAccountsLabel, string accountNumberLabel, string accountNameLabel, string budgetLabel, string postedLabel, string availableLabel, IValueDisplayer statusDate, bool budgetAccountCreationPossible, IReadOnlyCollection<IChartOfBudgetAccountsSectionDisplayer> sections)
	{
		ChartOfBudgetAccountsLabel = chartOfBudgetAccountsLabel;
		AccountNumberLabel = accountNumberLabel;
		AccountNameLabel = accountNameLabel;
		BudgetLabel = budgetLabel;
		PostedLabel = postedLabel;
		AvailableLabel = availableLabel;
		StatusDate = statusDate;
		BudgetAccountCreationPossible = budgetAccountCreationPossible;
		Sections = sections;
	}

	#endregion

	#region Properties

	public string ChartOfBudgetAccountsLabel { get; }

    public string AccountNumberLabel { get; }

    public string AccountNameLabel { get; }

    public string BudgetLabel { get; }

    public string PostedLabel { get; }

    public string AvailableLabel { get; }

    public IValueDisplayer StatusDate { get; }

    public bool BudgetAccountCreationPossible { get; }

    public IReadOnlyCollection<IChartOfBudgetAccountsSectionDisplayer> Sections { get; }

	#endregion

	#region Methods

	internal static async Task<IChartOfBudgetAccountsDisplayer> CreateAsync(IStaticTextProvider staticTextProvider, AccountingModel accounting, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
	{
		string chartOfBudgetAccountsLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BudgetAccounts, StaticTextKey.BudgetAccounts.DefaultArguments(), formatProvider, cancellationToken);
        string accountNumberLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.AccountNumberShort, StaticTextKey.AccountNumberShort.DefaultArguments(), formatProvider, cancellationToken);
        string accountNameLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments(), formatProvider, cancellationToken);
        string budgetLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Budget, StaticTextKey.Budget.DefaultArguments(), formatProvider, cancellationToken);
        string postedLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Posted, StaticTextKey.Posted.DefaultArguments(), formatProvider, cancellationToken);
        string availableLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Available, StaticTextKey.Available.DefaultArguments(), formatProvider, cancellationToken);

        string statusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.StatusDate, StaticTextKey.StatusDate.DefaultArguments(), formatProvider, cancellationToken);
        IValueDisplayer statusDate = new ValueDisplayer<DateTimeOffset>(statusDateLabel, accounting.StatusDate, formatProvider, (value, fp) => value.ToString("D", fp));

        IChartOfBudgetAccountsSectionDisplayer[] sections = accounting.BudgetAccounts.GroupBy(budgetAccount => budgetAccount.BudgetAccountGroup.Number)
            .Select(group => ChartOfBudgetAccountsSectionDisplayer.Create(group.First().BudgetAccountGroup, group.ToArray(), formatProvider))
            .ToArray();

		return new ChartOfBudgetAccountsDisplayer(chartOfBudgetAccountsLabel, accountNumberLabel, accountNameLabel, budgetLabel, postedLabel, availableLabel, statusDate, accounting.Modifiable, sections);
	}

	#endregion
}