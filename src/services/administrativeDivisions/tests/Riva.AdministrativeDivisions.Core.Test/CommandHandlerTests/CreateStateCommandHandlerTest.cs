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
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class CreateStateCommandHandlerTest
    {
        private readonly Mock<IStateRepository> _stateRepositoryMock;
        private readonly Mock<IStateVerificationService> _stateVerificationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICommandHandler<CreateStateCommand> _commandHandler;

        public CreateStateCommandHandlerTest()
        {
            _stateRepositoryMock = new Mock<IStateRepository>();
            _stateVerificationServiceMock = new Mock<IStateVerificationService>();
            _mapperMock = new Mock<IMapper>();
            _commandHandler = new CreateStateCommandHandler(_stateRepositoryMock.Object, _stateVerificationServiceMock.Object, _mapperMock.Object);
        }
        
        [Fact]
        public async Task HandleAsync_Should_Create_State()
        {
            var command = new CreateStateCommand(Guid.NewGuid(), "Name", "PolishName");
            var state = State.Builder()
                .SetId(command.StateId)
                .SetRowVersion(Array.Empty<byte>())
                .SetName(command.Name)
                .SetPolishName(command.PolishName)
                .Build();
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();

            _stateVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _stateVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);
            _mapperMock.Setup(x => x.Map<CreateStateCommand, State>(It.IsAny<CreateStateCommand>())).Returns(state);
            _stateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<State>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Name_And_PolishName_Are_Already_Used()
        {
            var command = new CreateStateCommand(Guid.NewGuid(), "Name", "PolishName");
            var duplicateNameError = new Error(StateErrorCodeEnumeration.NameAlreadyInUse, StateErrorMessage.NameAlreadyInUse);
            var duplicatePolishNameError = new Error(StateErrorCodeEnumeration.PolishNameAlreadyInUse, StateErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicateNameError, duplicatePolishNameError };

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
            var command = new CreateStateCommand(Guid.NewGuid(), "Name", "PolishName");
            var duplicateNameError = new Error(StateErrorCodeEnumeration.NameAlreadyInUse, StateErrorMessage.NameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError> { duplicateNameError };

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
            var command = new CreateStateCommand(Guid.NewGuid(), "Name", "PolishName");
            var duplicatePolishNameError = new Error(StateErrorCodeEnumeration.PolishNameAlreadyInUse, StateErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicatePolishNameError };

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