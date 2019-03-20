using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.CommandBus
{
    public interface ICommandBus
    {
        Task PublishAsync<TCommand>(TCommand command) where TCommand : ICommand;

        Task<TResult> PublishAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand;
    }
}