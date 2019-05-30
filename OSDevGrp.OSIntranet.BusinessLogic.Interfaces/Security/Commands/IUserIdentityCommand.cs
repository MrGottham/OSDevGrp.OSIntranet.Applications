using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IUserIdentityCommand : IIdentityCommand
    {
        string ExternalUserIdentifier { get; set; }

        IUserIdentity ToDomain();
    }
}