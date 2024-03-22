using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IAcmeChallengeCommand : ICommand
    {
        string ChallengeToken { get; }

        IValidator Validate(IValidator validator);
    }
}