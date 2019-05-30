using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IIdentityIdentificationCommand : ICommand
    {
        int Identifier { get; set; }

        IValidator Validate(IValidator validator, ISecurityRepository securityRepository);
    }
}