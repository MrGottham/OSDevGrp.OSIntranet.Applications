using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfAccountsLineDisplayer : IChartOfAccountsLineDisplayer
{
    #region Constructor

    private ChartOfAccountsLineDisplayer(string accountNumber, string accountName, string? credit, string? balance, string? available, bool modifiable, bool deletable)
    {
        AccountNumber = accountNumber;
        AccountName = accountName;
        Credit = credit;
        Balance = balance;
        Available = available;
        Modifiable = modifiable;
        Deletable = deletable;
    }

    #endregion

    #region Properties

    public string AccountNumber { get; }

    public string AccountName { get; }

    public string? Credit { get; }

    public string? Balance { get; }

    public string? Available { get; }

    public bool Modifiable { get; }

    public bool Deletable { get; }

    #endregion

    #region Methods

    internal static IChartOfAccountsLineDisplayer Create(AccountModel account, IFormatProvider formatProvider)
    {
        return new ChartOfAccountsLineDisplayer(
            account.AccountNumber, 
            account.AccountName, 
            account.ValuesAtStatusDate.Credit.ToString("C", formatProvider), 
            account.ValuesAtStatusDate.Balance.ToString("C", formatProvider), 
            account.ValuesAtStatusDate.Available.ToString("C", formatProvider), 
            account.Modifiable, 
            account.Deletable);
    }

    #endregion
}