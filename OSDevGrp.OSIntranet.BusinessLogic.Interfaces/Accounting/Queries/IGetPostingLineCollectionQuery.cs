namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries
{
    public interface IGetPostingLineCollectionQuery : IAccountingIdentificationQuery
    {
        int NumberOfPostingLines { get; set; }
    }
}