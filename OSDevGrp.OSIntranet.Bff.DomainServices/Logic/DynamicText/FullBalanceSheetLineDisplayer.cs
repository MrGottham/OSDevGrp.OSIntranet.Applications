using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class FullBalanceSheetLineDisplayer : IFullBalanceSheetLineDisplayer
{
    #region Constructor

    private FullBalanceSheetLineDisplayer(string? identification, string description, string? creditAtStatusDate, string? balanceAtStatusDate, string? availableAtStatusDate, string? creditAtEndOfLastMonthFromStatusDate, string? balanceAtEndOfLastMonthFromStatusDate, string? availableAtEndOfLastMonthFromStatusDate, string? creditAtEndOfLastYearFromStatusDate, string? balanceAtEndOfLastYearFromStatusDate, string? availableAtEndOfLastYearFromStatusDate)
    {
        Identification = identification;
        Description = description;
        CreditAtStatusDate = creditAtStatusDate;
        BalanceAtStatusDate = balanceAtStatusDate;
        AvailableAtStatusDate = availableAtStatusDate;
        CreditAtEndOfLastMonthFromStatusDate = creditAtEndOfLastMonthFromStatusDate;
        BalanceAtEndOfLastMonthFromStatusDate = balanceAtEndOfLastMonthFromStatusDate;
        AvailableAtEndOfLastMonthFromStatusDate = availableAtEndOfLastMonthFromStatusDate;
        CreditAtEndOfLastYearFromStatusDate = creditAtEndOfLastYearFromStatusDate;
        BalanceAtEndOfLastYearFromStatusDate = balanceAtEndOfLastYearFromStatusDate;
        AvailableAtEndOfLastYearFromStatusDate = availableAtEndOfLastYearFromStatusDate;
    }

    #endregion

    #region Properties

    public string? Identification { get; }

    public string Description { get; }

    public string? CreditAtStatusDate { get; }

    public string? BalanceAtStatusDate { get; }

    public string? AvailableAtStatusDate { get; }

    public string? CreditAtEndOfLastMonthFromStatusDate { get; }

    public string? BalanceAtEndOfLastMonthFromStatusDate { get; }

    public string? AvailableAtEndOfLastMonthFromStatusDate { get; }

    public string? CreditAtEndOfLastYearFromStatusDate { get; }

    public string? BalanceAtEndOfLastYearFromStatusDate { get; }

    public string? AvailableAtEndOfLastYearFromStatusDate { get; }

    #endregion

    #region Methods

    internal static IFullBalanceSheetLineDisplayer Create(string? identification, string description, IEnumerable<AccountModel> accounts, IFormatProvider formatProvider)
    {
        return new FullBalanceSheetLineDisplayer(
            identification, 
            description, 
            accounts.Sum(account => account.ValuesAtStatusDate.Credit).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtStatusDate.Balance).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtStatusDate.Available).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtEndOfLastMonthFromStatusDate.Credit).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtEndOfLastMonthFromStatusDate.Balance).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtEndOfLastMonthFromStatusDate.Available).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtEndOfLastYearFromStatusDate.Credit).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtEndOfLastYearFromStatusDate.Balance).ToString("C", formatProvider),
            accounts.Sum(account => account.ValuesAtEndOfLastYearFromStatusDate.Available).ToString("C", formatProvider));
    }

    #endregion
}