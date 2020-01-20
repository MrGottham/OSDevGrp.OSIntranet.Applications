namespace OSDevGrp.OSIntranet.Core.Interfaces.Commands
{
    public interface IRefreshableTokenBasedCommand : ITokenBasedCommand
    {
        string RefreshToken { get; set; }
    }
}