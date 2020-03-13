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
    public class UpdateStateCommandHandlerTest
    {
        private readonly Mock<IStateRepository> _stateRepositoryMock;
        private readonly Mock<IStateGetterService> _stateGetterServiceMock;
        private readonly Mock<IStateVerificationService> _stateVerificationServiceMock;
        private readonly ICommandHandler<UpdateStateCommand> _commandHandler;

        public UpdateStateCommandHandlerTest()
        {
            _stateRepositoryMock = new Mock<IStateRepository>();
            _stateGetterServiceMock = new Mock<IStateGetterService>();
            _stateVerificationServiceMock = new Mock<IStateVerificationService>();
            _commandHandler = new UpdateStateCommandHandler(_stateRepositoryMock.Object, _stateGetterServiceMock.Object,
                _stateVerificationServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_State()
        {
            var command = new UpdateStateCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName");
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getStateResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);
            _stateRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<State>())).Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
            _stateRepositoryMock.Verify(x =>
                x.UpdateAsync(It.Is<State>(s => s.Name.Equals(state.Name) && s.PolishName.Equals(state.PolishName))));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_PreconditionFailedException_When_RowVersion_Is_Incorrect()
        {
            var command = new UpdateStateCommand(Guid.NewGuid(), new byte[] {1, 2, 4, 8, 16, 32, 64}, "NewName",
                "NewPolishName");
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(new byte[] { 1, 2, 4, 8, 16, 32, 128 })
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getStateResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().ThrowExactlyAsync<PreconditionFailedException>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_State_Does_Not_Exist()
        {
            var command = new UpdateStateCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName");
            var errors = new Collection<IError>
            {
                new Error(StateErrorCodeEnumeration.NotFound, StateErrorMessage.NotFound)
            };
            var getStateResult = GetResult<State>.Fail(errors);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Name_And_PolishName_Are_Already_Used()
        {
            var command = new UpdateStateCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName");
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var duplicateNameError = new Error(StateErrorCodeEnumeration.NameAlreadyInUse, StateErrorMessage.NameAlreadyInUse);
            var duplicatePolishNameError = new Error(StateErrorCodeEnumeration.PolishNameAlreadyInUse, StateErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicateNameError, duplicatePolishNameError };

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Only_Name_Is_Already_Used()
        {
            var command = new UpdateStateCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName");
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var duplicateNameError = new Error(StateErrorCodeEnumeration.NameAlreadyInUse, StateErrorMessage.NameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError> { duplicateNameError };

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Only_PolishName_Is_Already_Used()
        {
            var command = new UpdateStateCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName");
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var duplicatePolishNameError = new Error(StateErrorCodeEnumeration.PolishNameAlreadyInUse, StateErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicatePolishNameError };

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}