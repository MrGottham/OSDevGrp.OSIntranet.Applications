namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountingIdentificationState
    {
        public AccountingIdentificationState(int accountingIdentifier)
        {
            AccountingIdentifier = accountingIdentifier;
        }

        public int AccountingIdentifier { get; }
    }
}