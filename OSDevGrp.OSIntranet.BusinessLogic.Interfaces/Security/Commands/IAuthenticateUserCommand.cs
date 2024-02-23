namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
	public interface IAuthenticateUserCommand : IAuthenticateCommand
    {
        string ExternalUserIdentifier { get; }
    }
}