using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IClientSecretIdentityCommand : IIdentityCommand
    {
        string FriendlyName { get; set; }

        IClientSecretIdentity ToDomain();

        IClientSecretIdentity ToDomain(string clientId, string clientSecret);
    }
}