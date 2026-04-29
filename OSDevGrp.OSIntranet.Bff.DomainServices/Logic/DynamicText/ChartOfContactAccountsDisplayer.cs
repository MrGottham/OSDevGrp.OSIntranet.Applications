using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfContactAccountsDisplayer : IChartOfContactAccountsDisplayer
{
	#region Constructor

	private ChartOfContactAccountsDisplayer(string chartOfContactAccountsLabel, string accountNumberLabel, string accountNameLabel, string balanceLabel, IValueDisplayer statusDate, bool contactAccountCreationPossible, IReadOnlyCollection<IChartOfContactAccountsLineDisplayer> lines)
	{
		ChartOfContactAccountsLabel = chartOfContactAccountsLabel;
        AccountNumberLabel = accountNumberLabel;
        AccountNameLabel = accountNameLabel;
		BalanceLabel = balanceLabel;
		StatusDate = statusDate;
		ContactAccountCreationPossible = contactAccountCreationPossible;
		Lines = lines;
	}

	#endregion

	#region Properties

	public string ChartOfContactAccountsLabel { get; }

    public string AccountNumberLabel { get; }

    public string AccountNameLabel { get; }

    public string BalanceLabel { get; }

    public IValueDisplayer StatusDate { get; }

    public bool ContactAccountCreationPossible { get; }

    public IReadOnlyCollection<IChartOfContactAccountsLineDisplayer> Lines { get; }

	#endregion

	#region Methods

	internal static async Task<IChartOfContactAccountsDisplayer> CreateAsync(IStaticTextProvider staticTextProvider, AccountingModel accounting, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
	{
		string chartOfContactAccountsLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.ContactAccounts, StaticTextKey.ContactAccounts.DefaultArguments(), formatProvider, cancellationToken);
        string accountNumberLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.AccountNumberShort, StaticTextKey.AccountNumberShort.DefaultArguments(), formatProvider, cancellationToken);
        string accountNameLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments(), formatProvider, cancellationToken);
        string balanceLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Balance, StaticTextKey.Balance.DefaultArguments(), formatProvider, cancellationToken);

        string statusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.StatusDate, StaticTextKey.StatusDate.DefaultArguments(), formatProvider, cancellationToken);
        IValueDisplayer statusDate = new ValueDisplayer<DateTimeOffset>(statusDateLabel, accounting.StatusDate, formatProvider, (value, fp) => value.ToString("D", fp));

		return new ChartOfContactAccountsDisplayer(chartOfContactAccountsLabel, accountNumberLabel, accountNameLabel, balanceLabel, statusDate, accounting.Modifiable, accounting.ContactAccounts.Select(contactAccount => ChartOfContactAccountsLineDisplayer.Create(contactAccount, formatProvider)).ToArray());
	}

	#endregion
}