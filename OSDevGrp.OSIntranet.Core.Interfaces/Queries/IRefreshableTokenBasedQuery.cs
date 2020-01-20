namespace OSDevGrp.OSIntranet.Core.Interfaces.Queries
{
    public interface IRefreshableTokenBasedQuery : ITokenBasedQuery
    {
        string RefreshToken { get; set; }
    }
}