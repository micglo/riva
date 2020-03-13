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
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class CreateCityCommandHandlerTest
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<IStateGetterService> _stateGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICommandHandler<CreateCityCommand> _commandHandler;

        public CreateCityCommandHandlerTest()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _stateGetterServiceMock = new Mock<IStateGetterService>();
            _mapperMock = new Mock<IMapper>();
            _commandHandler = new CreateCityCommandHandler(_cityRepositoryMock.Object, _cityVerificationServiceMock.Object,
                _stateGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_City()
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var command = new CreateCityCommand(Guid.NewGuid(), "Name", "PolishName", state.Id);
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(Array.Empty<byte>())
                .SetName(command.Name)
                .SetPolishName(command.PolishName)
                .SetStateId(command.StateId)
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);
            _mapperMock.Setup(x => x.Map<CreateCityCommand, City>(It.IsAny<CreateCityCommand>())).Returns(city);
            _cityRepositoryMock.Setup(x => x.AddAsync(It.IsAny<City>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_State_Is_Not_Found()
        {
            var stateId = Guid.NewGuid();
            var command = new CreateCityCommand(Guid.NewGuid(), "Name", "PolishName", stateId);
            var errors = new Collection<IError>
            {
                new Error(StateErrorCodeEnumeration.NotFound, StateErrorMessage.NotFound)
            };
            var getStateResult = GetResult<State>.Fail(errors);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Name_And_PolishName_Are_Already_Used()
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var command = new CreateCityCommand(Guid.NewGuid(), "Name", "PolishName", Guid.NewGuid());
            var duplicateNameError = new Error(CityErrorCodeEnumeration.NameAlreadyInUse, CityErrorMessage.NameAlreadyInUse);
            var duplicatePolishNameError = new Error(CityErrorCodeEnumeration.PolishNameAlreadyInUse, CityErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicateNameError, duplicatePolishNameError };


            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Only_Name_Is_Already_Used()
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var command = new CreateCityCommand(Guid.NewGuid(), "Name", "PolishName", Guid.NewGuid());
            var duplicateNameError = new Error(CityErrorCodeEnumeration.NameAlreadyInUse, CityErrorMessage.NameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError> { duplicateNameError };


            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Only_PolishName_Is_Already_Used()
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var command = new CreateCityCommand(Guid.NewGuid(), "Name", "PolishName", Guid.NewGuid());
            var duplicatePolishNameError = new Error(CityErrorCodeEnumeration.PolishNameAlreadyInUse, CityErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicatePolishNameError };


            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}