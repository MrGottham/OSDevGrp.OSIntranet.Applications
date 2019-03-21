using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandHandlers.CommandHandlerNonTransactionalBase
{
    public abstract class CommandHandlerNonTransactionalTestBase
    {
        protected ICommandHandler CreateSut()
        {
            return new CommandHandlerNonTransactional();
        }
    }
}