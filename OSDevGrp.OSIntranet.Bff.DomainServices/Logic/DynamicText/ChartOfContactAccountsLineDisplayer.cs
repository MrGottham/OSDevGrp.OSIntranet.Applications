using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfContactAccountsLineDisplayer : IChartOfContactAccountsLineDisplayer
{
    #region Constructor

    private ChartOfContactAccountsLineDisplayer(string accountNumber, string accountName, string? balance, bool modifiable, bool deletable)
    {
        AccountNumber = accountNumber;
        AccountName = accountName;
        Balance = balance;
        Modifiable = modifiable;
        Deletable = deletable;
    }

    #endregion

    #region Properties

    public string AccountNumber { get; }

    public string AccountName { get; }

    public string? Balance { get; }

    public bool Modifiable { get; }

    public bool Deletable { get; }

    #endregion

    #region Methods

    internal static IChartOfContactAccountsLineDisplayer Create(ContactAccountModel contactAccount, IFormatProvider formatProvider)
    {
        return new ChartOfContactAccountsLineDisplayer(
            contactAccount.AccountNumber, 
            contactAccount.AccountName, 
            contactAccount.ValuesAtStatusDate.Balance.ToString("C", formatProvider), 
            contactAccount.Modifiable, 
            contactAccount.Deletable);
    }

    #endregion
}