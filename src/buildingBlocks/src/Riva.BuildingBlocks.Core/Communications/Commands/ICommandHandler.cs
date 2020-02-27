using System.Threading;
using System.Threading.Tasks;

namespace Riva.BuildingBlocks.Core.Communications.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}