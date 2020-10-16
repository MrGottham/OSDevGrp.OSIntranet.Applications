namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries
{
    public interface IAccountIdentificationQuery : IAccountingIdentificationQuery
    {
        string AccountNumber { get; set; }
    }
}