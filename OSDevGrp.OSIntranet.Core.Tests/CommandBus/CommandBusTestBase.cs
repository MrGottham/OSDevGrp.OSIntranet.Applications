using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using OSDevGrp.OSIntranet.Core.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandBus
{
    public abstract class CommandBusTestBase
    {
        protected ICommandBus CreateSut(IEnumerable<ICommandHandler> commandHandlerCollection = null, Exception exception = null)
        {
            return new Core.CommandBus(commandHandlerCollection ?? CreateCommandHandlerMockCollection(exception: exception));
        }

        protected IEnumerable<ICommandHandler> CreateCommandHandlerMockCollection(ICommandHandler commandHandler = null, Exception exception = null)
        {
            IList<ICommandHandler> commandHandlerCollection = new List<ICommandHandler>
            {
                CreateCommandHandlerMockWithoutResult<EmptyCommand>(exception).Object,
                CreateCommandHandlerMockWithResult<EmptyCommand, object>(new object(), exception).Object
            };

            if (commandHandlerCollection == null)
            {
                return commandHandlerCollection;
            }

            commandHandlerCollection.Add(commandHandler);
            return commandHandlerCollection;
        }

        protected Mock<ICommandHandler<TCommand>> CreateCommandHandlerMockWithoutResult<TCommand>(Exception exception = null) where TCommand : ICommand
        {
            Mock<ICommandHandler<TCommand>> commandHandlerMock = new Mock<ICommandHandler<TCommand>>();
            if (exception == null)
            {
                commandHandlerMock.Setup(m => m.ExecuteAsync(It.IsAny<TCommand>()))
                    .Returns(Task.Run(() => { }));
            }
            else
            {
                commandHandlerMock.Setup(m => m.ExecuteAsync(It.IsAny<TCommand>()))
                    .Throws(exception);
            }
            return commandHandlerMock;
        }

        protected Mock<ICommandHandler<TCommand, TResult>> CreateCommandHandlerMockWithResult<TCommand, TResult>(TResult result, Exception exception = null) where TCommand : ICommand
        {
            Mock<ICommandHandler<TCommand, TResult>> commandHandlerMock = new Mock<ICommandHandler<TCommand, TResult>>();
            if (exception == null)
            {
                commandHandlerMock.Setup(m => m.ExecuteAsync(It.IsAny<TCommand>()))
                    .Returns(Task.Run(() => result));
            }
            else
            {
                commandHandlerMock.Setup(m => m.ExecuteAsync(It.IsAny<TCommand>()))
                    .Throws(exception);
            }
            return commandHandlerMock;
        }
    }
}