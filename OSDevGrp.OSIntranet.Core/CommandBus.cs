using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core
{
    public class CommandBus : BusBase, ICommandBus
    {
        #region Private variabels

        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        #endregion

        #region Constructor

        public CommandBus(IEnumerable<ICommandHandler> commandHandlers)
        {
            NullGuard.NotNull(commandHandlers, nameof(commandHandlers));

            _commandHandlers = commandHandlers;
        }

        #endregion

        #region Methods

        public Task PublishAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            NullGuard.NotNull(command, nameof(command));

            try
            {
                ICommandHandler<TCommand> commandHandler = _commandHandlers.OfType<ICommandHandler<TCommand>>()
                    .Cast<ICommandHandler<TCommand>>()
                    .SingleOrDefault();
                if (commandHandler == null)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.NoCommandHandlerSupportingCommandWithoutResultType, command.GetType().Name).Build();
                }
                
                return commandHandler.ExecuteAsync(command);
            }
            catch (IntranetExceptionBase)
            {
                throw;
            }
            catch (AggregateException aggregateException)
            {
                throw Handle(aggregateException, innerException => new IntranetExceptionBuilder(ErrorCode.ErrorWhilePublishingCommandWithoutResultType, command.GetType().Name, innerException.Message).WithInnerException(innerException));
            }
            catch (Exception ex)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ErrorWhilePublishingCommandWithoutResultType, command.GetType().Name, ex.Message)
                    .WithInnerException(ex)
                    .Build();
            }
        }

        public Task<TResult> PublishAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand
        {
            NullGuard.NotNull(command, nameof(command));
            
            try
            {
                ICommandHandler<TCommand, TResult> commandHandler = _commandHandlers.OfType<ICommandHandler<TCommand, TResult>>()
                    .Cast<ICommandHandler<TCommand, TResult>>()
                    .SingleOrDefault();
                if (commandHandler == null)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.NoCommandHandlerSupportingCommandWithResultType, command.GetType().Name, typeof(TResult).Name).Build();
                }

                return commandHandler.ExecuteAsync(command);
            }
            catch (IntranetExceptionBase)
            {
                throw;
            }
            catch (AggregateException aggregateException)
            {
                throw Handle(aggregateException, innerException => new IntranetExceptionBuilder(ErrorCode.ErrorWhilePublishingCommandWithResultType, command.GetType().Name, typeof(TResult).Name, innerException.Message).WithInnerException(innerException));
            }
            catch (Exception ex)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ErrorWhilePublishingCommandWithResultType, command.GetType().Name, typeof(TResult).Name, ex.Message)
                    .WithInnerException(ex)
                    .Build();
            }
        }

        #endregion 
    }
}