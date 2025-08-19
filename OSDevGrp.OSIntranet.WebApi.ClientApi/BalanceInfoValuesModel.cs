namespace OSDevGrp.OSIntranet.WebApi.ClientApi;

public partial class BalanceInfoValuesModel
{
    #region Methods

    public bool IsDebtor(BalanceBelowZeroType balanceBelowZero)
    {
        if (balanceBelowZero == BalanceBelowZeroType.Debtors)
        {
            return Balance < 0;
        }

        return Balance > 0;
    }

    public bool IsCreditor(BalanceBelowZeroType balanceBelowZero)
    {
        if (balanceBelowZero == BalanceBelowZeroType.Creditors)
        {
            return Balance < 0;
        }

        return Balance > 0;
    }

    #endregion
}