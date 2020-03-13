using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Commands.Handlers;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class DeleteStateCommandHandlerTest
    {
        private readonly Mock<IStateGetterService> _stateGetterServiceMock;
        private readonly Mock<IStateRepository> _stateRepositoryMock;
        private readonly ICommandHandler<DeleteStateCommand> _commandHandler;

        public DeleteStateCommandHandlerTest()
        {
            _stateGetterServiceMock = new Mock<IStateGetterService>();
            _stateRepositoryMock = new Mock<IStateRepository>();
            _commandHandler = new DeleteStateCommandHandler(_stateGetterServiceMock.Object, _stateRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_State()
        {
            var command = new DeleteStateCommand(Guid.NewGuid(), Array.Empty<byte>());
            var state = State.Builder()
                .SetId(command.StateId)
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _stateRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<State>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);

            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_State_Is_Not_Found()
        {
            var command = new DeleteStateCommand(Guid.NewGuid(), new byte[] { 1, 2, 4, 8, 16, 32, 64 });
            var errors = new Collection<IError>
            {
                new Error(StateErrorCodeEnumeration.NotFound, StateErrorMessage.NotFound)
            };
            var getStateResult = GetResult<State>.Fail(errors);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_PreconditionFailedException_When_RowVersion_Does_Not_Match()
        {
            var command = new DeleteStateCommand(Guid.NewGuid(), new byte[] { 1, 2, 4, 8, 16, 64 });
            var state = State.Builder()
                .SetId(command.StateId)
                .SetRowVersion(new byte[] { 1, 2, 4, 8, 16, 32 })
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);

            await result.Should().ThrowAsync<PreconditionFailedException>();
        }
    }
}