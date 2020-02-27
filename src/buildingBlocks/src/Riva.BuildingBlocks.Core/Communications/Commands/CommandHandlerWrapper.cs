using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riva.BuildingBlocks.Core.Communications.Commands
{
    public abstract class CommandHandlerWrapperBase
    {
        public abstract Task HandleAsync(ICommand command, CancellationToken cancellationToken, ServiceFactory serviceFactory,
            Func<IEnumerable<Func<ICommand, CancellationToken, Task>>, ICommand, CancellationToken, Task> send);
    }

    public class CommandHandlerWrapper<TCommand> : CommandHandlerWrapperBase where TCommand : ICommand
    {
        public override Task HandleAsync(ICommand command, CancellationToken cancellationToken, ServiceFactory serviceFactory,
            Func<IEnumerable<Func<ICommand, CancellationToken, Task>>, ICommand, CancellationToken, Task> send)
        {
            var handlers = serviceFactory
                .GetInstances<ICommandHandler<TCommand>>()
                .Select(x =>
                    new Func<ICommand, CancellationToken, Task>((theCommand, theToken) =>
                        x.HandleAsync((TCommand) theCommand, theToken)));

            return send(handlers, command, cancellationToken);
        }
    }
}