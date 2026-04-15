using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class FullBalanceSheetDisplayer : IFullBalanceSheetDisplayer
{
    #region Constructor

    private FullBalanceSheetDisplayer(string balanceSheetLabel, string balanceSheetAtStatusDateLabel, string balanceSheetAtEndOfLastMonthFromStatusDateLabel, string balanceSheetAtEndOfLastYearFromStatusDateLabel, string assetsLabel, string liabilitiesLabel, IValueDisplayer statusDate, IReadOnlyCollection<IFullBalanceSheetLineDisplayer> assetsLines, IReadOnlyCollection<IFullBalanceSheetLineDisplayer> liabilitiesLines)
    {
        BalanceSheetLabel = balanceSheetLabel;
        BalanceSheetAtStatusDateLabel = balanceSheetAtStatusDateLabel;
        BalanceSheetAtEndOfLastMonthFromStatusDateLabel = balanceSheetAtEndOfLastMonthFromStatusDateLabel;
        BalanceSheetAtEndOfLastYearFromStatusDateLabel = balanceSheetAtEndOfLastYearFromStatusDateLabel;
        AssetsLabel = assetsLabel;
        LiabilitiesLabel = liabilitiesLabel;
        StatusDate = statusDate;
        AssetsLines = assetsLines;
        LiabilitiesLines = liabilitiesLines;
    }

    #endregion

    #region Properties

    public string BalanceSheetLabel { get; }

    public string BalanceSheetAtStatusDateLabel { get; }

    public string BalanceSheetAtEndOfLastMonthFromStatusDateLabel { get; }

    public string BalanceSheetAtEndOfLastYearFromStatusDateLabel { get; }

    public string AssetsLabel { get; }

    public string LiabilitiesLabel { get; }

    public IValueDisplayer StatusDate { get; }

    public IReadOnlyCollection<IFullBalanceSheetLineDisplayer> AssetsLines { get; }

    public IReadOnlyCollection<IFullBalanceSheetLineDisplayer> LiabilitiesLines { get; }

    #endregion

    #region Methods

    internal static async Task<IFullBalanceSheetDisplayer> CreateAsync(IStaticTextProvider staticTextProvider, AccountingModel accounting, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string balanceSheetLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BalanceSheet, StaticTextKey.BalanceSheet.DefaultArguments(), formatProvider, cancellationToken);
        string balanceSheetAtStatusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BalanceSheetAtStatusDate, StaticTextKey.BalanceSheetAtStatusDate.DefaultArguments(), formatProvider, cancellationToken);
        string balanceSheetAtEndOfLastMonthFromStatusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate, StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate.DefaultArguments(), formatProvider, cancellationToken);
        string balanceSheetAtEndOfLastYearFromStatusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.BalanceSheetAtEndOfLastYearFromStatusDate, StaticTextKey.BalanceSheetAtEndOfLastYearFromStatusDate.DefaultArguments(), formatProvider, cancellationToken);
        string assetsLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Assets, StaticTextKey.Assets.DefaultArguments(), formatProvider, cancellationToken);
        string liabilitiesLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Liabilities, StaticTextKey.Liabilities.DefaultArguments(), formatProvider, cancellationToken);

        string statusDateLabel = await staticTextProvider.GetStaticTextAsync(StaticTextKey.StatusDate, StaticTextKey.StatusDate.DefaultArguments(), formatProvider, cancellationToken);
        IValueDisplayer statusDate = new ValueDisplayer<DateTimeOffset>(statusDateLabel, accounting.StatusDate, formatProvider, (value, fp) => value.ToString("D", fp));

        IReadOnlyCollection<IFullBalanceSheetLineDisplayer> assetsLines = await GenerateLinesAsync(accounting.Accounts, AccountGroupType.Assets, StaticTextKey.AssetsTotal, staticTextProvider, formatProvider, cancellationToken);
        IReadOnlyCollection<IFullBalanceSheetLineDisplayer> liabilitiesLines = await GenerateLinesAsync(accounting.Accounts, AccountGroupType.Liabilities, StaticTextKey.LiabilitiesTotal, staticTextProvider, formatProvider, cancellationToken);

        return new FullBalanceSheetDisplayer(balanceSheetLabel, balanceSheetAtStatusDateLabel, balanceSheetAtEndOfLastMonthFromStatusDateLabel, balanceSheetAtEndOfLastYearFromStatusDateLabel, assetsLabel, liabilitiesLabel, statusDate, assetsLines, liabilitiesLines);
    }

    private static async Task<IReadOnlyCollection<IFullBalanceSheetLineDisplayer>> GenerateLinesAsync(IEnumerable<AccountModel> accounts, AccountGroupType accountGroupType, StaticTextKey total, IStaticTextProvider staticTextProvider, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        AccountModel[] filteredAccounts = accounts.Where(account => account.AccountGroup.AccountGroupType == accountGroupType).ToArray();
        if (filteredAccounts.Length == 0)
        {
            return Array.Empty<IFullBalanceSheetLineDisplayer>();
        }

        IFullBalanceSheetLineDisplayer[] lines = filteredAccounts
            .GroupBy(filteredAccount => filteredAccount.AccountGroup.Number)
            .OrderBy(group => group.Key)
            .Select(group => FullBalanceSheetLineDisplayer.Create(group.Key.ToString(formatProvider), group.First().AccountGroup.Name, group, formatProvider))
            .ToArray();

        string totalLabel = await staticTextProvider.GetStaticTextAsync(total, total.DefaultArguments(), formatProvider, cancellationToken);
        IFullBalanceSheetLineDisplayer totalLine = FullBalanceSheetLineDisplayer.Create(null, totalLabel, filteredAccounts, formatProvider);

        return lines.Concat([totalLine]).ToArray();
    }

    #endregion
}