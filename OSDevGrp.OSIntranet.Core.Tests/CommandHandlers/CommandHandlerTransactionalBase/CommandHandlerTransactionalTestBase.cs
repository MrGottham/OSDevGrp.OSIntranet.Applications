using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandHandlers.CommandHandlerTransactionalBase
{
    public abstract class CommandHandlerTransactionalTestBase
    {
        protected ICommandHandler CreateSut()
        {
            return new CommandHandlerTransactional();
        }
    }
}