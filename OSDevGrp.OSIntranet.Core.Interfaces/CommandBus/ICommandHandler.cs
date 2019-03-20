using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.CommandBus
{
    public interface ICommandHandler : IUnitOfWorkAwareable
    {
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }

    public interface ICommandHandler<in TCommand, TResult> : ICommandHandler where TCommand : ICommand
    {
        Task<TResult> ExecuteAsync(TCommand command);
    }
}