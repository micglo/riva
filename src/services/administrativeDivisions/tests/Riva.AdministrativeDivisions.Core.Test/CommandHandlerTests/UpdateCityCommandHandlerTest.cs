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
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class UpdateCityCommandHandlerTest
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly Mock<ICityGetterService> _cityGetterServiceMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<IStateGetterService> _stateGetterServiceMock;
        private readonly ICommandHandler<UpdateCityCommand> _commandHandler;

        public UpdateCityCommandHandlerTest()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _cityGetterServiceMock = new Mock<ICityGetterService>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _stateGetterServiceMock = new Mock<IStateGetterService>();
            _commandHandler = new UpdateCityCommandHandler(_cityRepositoryMock.Object, _cityGetterServiceMock.Object,
                _cityVerificationServiceMock.Object, _stateGetterServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_City()
        {
            var command = new UpdateCityCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName",
                Guid.NewGuid());
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var state = State.Builder()
                .SetId(command.StateId)
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);
            _cityRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<City>())).Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
            _cityRepositoryMock.Verify(x =>
                x.UpdateAsync(It.Is<City>(s =>
                    s.Name.Equals(command.Name) && s.PolishName.Equals(command.PolishName) && s.StateId.Equals(command.StateId))));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_City_Is_Not_Found()
        {
            var command = new UpdateCityCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName",
                Guid.NewGuid());
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var getCityResult = GetResult<City>.Fail(errors);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_PreconditionFailedException_When_RowVersion_Is_Incorrect()
        {
            var command = new UpdateCityCommand(Guid.NewGuid(), new byte[] {1, 2, 4, 8, 16, 32, 64}, "NewName",
                "NewPolishName", Guid.NewGuid());
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(new byte[] { 1, 2, 4, 8, 16, 32, 128 })
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().ThrowExactlyAsync<PreconditionFailedException>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_State_Is_Not_Found()
        {
            var command = new UpdateCityCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName",
                Guid.NewGuid());
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var errors = new Collection<IError>
            {
                new Error(StateErrorCodeEnumeration.NotFound, StateErrorMessage.NotFound)
            };
            var getStateResult = GetResult<State>.Fail(errors);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Name_And_PolishName_Are_Already_Used()
        {
            var command = new UpdateCityCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName",
                Guid.NewGuid());
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var state = State.Builder()
                .SetId(command.StateId)
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var duplicateNameError = new Error(CityErrorCodeEnumeration.NameAlreadyInUse, CityErrorMessage.NameAlreadyInUse);
            var duplicatePolishNameError = new Error(CityErrorCodeEnumeration.PolishNameAlreadyInUse, CityErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicateNameError, duplicatePolishNameError };

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
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
            var command = new UpdateCityCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName",
                Guid.NewGuid());
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var state = State.Builder()
                .SetId(command.StateId)
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var duplicateNameError = new Error(CityErrorCodeEnumeration.NameAlreadyInUse, CityErrorMessage.NameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicateNameError });
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError> { duplicateNameError };

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
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
            var command = new UpdateCityCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName", "NewPolishName",
                Guid.NewGuid());
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(command.RowVersion)
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var state = State.Builder()
                .SetId(command.StateId)
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var duplicatePolishNameError = new Error(CityErrorCodeEnumeration.PolishNameAlreadyInUse, CityErrorMessage.PolishNameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { duplicatePolishNameError });
            var errors = new Collection<IError> { duplicatePolishNameError };

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
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