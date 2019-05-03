namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IRefreshableToken : IToken
    {
        string RefreshToken { get; }
    }
}
