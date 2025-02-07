namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Options;

internal class AccountingTestOptions
{
    public required int ExistingAccountingNumber { get; set; }

    public required string ExistingAccountNumberForAccount { get; set; }

    public required string ExistingAccountNumberForBudgetAccount { get; set; }

    public required string ExistingAccountNumberForContactAccount { get; set; }
}