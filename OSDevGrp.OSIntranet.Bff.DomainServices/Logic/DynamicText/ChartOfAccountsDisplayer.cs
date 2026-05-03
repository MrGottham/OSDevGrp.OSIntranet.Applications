using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfAccountsDisplayer : IChartOfAccountsDisplayer
{
	#region Constructor

    private ChartOfAccountsDisplayer(string chartOfAccountsLabel, string accountNumberLabel, string accountNameLabel, string creditLabel, string balanceLabel, string availableLabel, IValueDisplayer statusDate, bool accountCreationPossible, IReadOnlyCollection<IChartOfAccountsSectionDisplayer> sections)
    {
        ChartOfAccountsLabel = chartOfAccountsLabel;
        AccountNumberLabel = accountNumberLabel;
        AccountNameLabel = accountNameLabel;
        CreditLabel = creditLabel;
        BalanceLabel = balanceLabel;
        AvailableLabel = availableLabel;
        StatusDate = statusDate;
        AccountCreationPossible = accountCreationPossible;
        Sections = sections;
    }

    #endregion

    #region Properties

    public string ChartOfAccountsLabel { get; }

    public string AccountNumberLabel { get; }

    public string AccountNameLabel { get; }

    public string CreditLabel { get; }

    public string BalanceLabel { get; }

    public string AvailableLabel { get; }

    public IValueDisplayer StatusDate { get; }

    public bool AccountCreationPossible { get; }

    public IReadOnlyCollection<IChartOfAccountsSectionDisplayer> Sections { get; }

    #endregion

    #region Methods

    internal static async Task<IChartOfAccountsDisplayer> CreateAsync(IStaticTextProvider staticTextProvider, AccountingModel accounting, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string chartOfAccountsLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Accounts, StaticTextKey.Accounts.DefaultArguments(), formatProvider, cancellationToken);
        string accountNumberLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.AccountNumberShort, StaticTextKey.AccountNumberShort.DefaultArguments(), formatProvider, cancellationToken);
        string accountNameLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments(), formatProvider, cancellationToken);
        string creditLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Credit, StaticTextKey.Credit.DefaultArguments(), formatProvider, cancellationToken);
        string balanceLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Balance, StaticTextKey.Balance.DefaultArguments(), formatProvider, cancellationToken);
        string availableLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Available, StaticTextKey.Available.DefaultArguments(), formatProvider, cancellationToken);

        string statusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.StatusDate, StaticTextKey.StatusDate.DefaultArguments(), formatProvider, cancellationToken);
        IValueDisplayer statusDate = new ValueDisplayer<DateTimeOffset>(statusDateLabel, accounting.StatusDate, formatProvider, (value, fp) => value.ToString("D", fp));

        IChartOfAccountsSectionDisplayer[] sections = accounting.Accounts.GroupBy(account => account.AccountGroup.Number)
            .Select(group => ChartOfAccountsSectionDisplayer.Create(group.First().AccountGroup, group.ToArray(), formatProvider))
            .ToArray();

        return new ChartOfAccountsDisplayer(chartOfAccountsLabel, accountNumberLabel, accountNameLabel, creditLabel, balanceLabel, availableLabel, statusDate, accounting.Modifiable, sections);
    }

    #endregion
}