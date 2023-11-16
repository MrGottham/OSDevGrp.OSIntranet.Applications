namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
	public interface IAuthenticateClientSecretCommand : IAuthenticateCommand
    {
        string ClientId { get; }

        string ClientSecret { get; }
    }
}