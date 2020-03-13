using System;
using System.Collections.Generic;
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
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class UpdateCityDistrictCommandHandlerTest
    {
        private readonly Mock<ICityDistrictGetterService> _cityDistrictGetterServiceMock;
        private readonly Mock<ICityGetterService> _cityGetterServiceMock;
        private readonly Mock<ICityDistrictVerificationService> _cityDistrictVerificationServiceMock;
        private readonly Mock<ICityDistrictRepository> _cityDistrictRepositoryMock;
        private readonly ICommandHandler<UpdateCityDistrictCommand> _commandHandler;

        public UpdateCityDistrictCommandHandlerTest()
        {
            _cityDistrictGetterServiceMock = new Mock<ICityDistrictGetterService>();
            _cityGetterServiceMock = new Mock<ICityGetterService>();
            _cityDistrictVerificationServiceMock = new Mock<ICityDistrictVerificationService>();
            _cityDistrictRepositoryMock = new Mock<ICityDistrictRepository>();
            _commandHandler = new UpdateCityDistrictCommandHandler(_cityDistrictGetterServiceMock.Object,
                _cityGetterServiceMock.Object, _cityDistrictVerificationServiceMock.Object,
                _cityDistrictRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_CityDistrict()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .Build();
            var getCityDistrict = GetResult<CityDistrict>.Ok(cityDistrict);
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var parentExistsVerificationResult = VerificationResult.Ok();
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();
            var command = new UpdateCityDistrictCommand(cityDistrict.Id, cityDistrict.RowVersion, "NewName",
                "NewPolishName", city.Id, Guid.NewGuid(), new Collection<string>{ "NewNameVariant" });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);
            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _cityDistrictVerificationServiceMock.Setup(x => x.VerifyParentExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(parentExistsVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);
            _cityDistrictRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CityDistrict>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_CityDistrict_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var getCityDistrict = GetResult<CityDistrict>.Fail(errors);
            var command = new UpdateCityDistrictCommand(Guid.NewGuid(), Array.Empty<byte>(), "Name", "PolishName",
                Guid.NewGuid(), Guid.NewGuid(), new Collection<string>{ "NameVariant" });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_PreconditionFailedException_When_RowVersion_Does_Not_Match()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(new byte[] { 1, 2, 4, 8, 16, 32, 64 })
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .Build();
            var getCityDistrict = GetResult<CityDistrict>.Ok(cityDistrict);
            var command = new UpdateCityDistrictCommand(cityDistrict.Id, new byte[] { 1, 2, 4, 8, 16, 32, 128 }, "NewName",
                "NewPolishName", Guid.NewGuid(), Guid.NewGuid(), new Collection<string> { "NewNameVariant" });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().ThrowExactlyAsync<PreconditionFailedException>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_Is_Not_Found()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .Build();
            var getCityDistrict = GetResult<CityDistrict>.Ok(cityDistrict);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var getCityResult = GetResult<City>.Fail(errors);
            var command = new UpdateCityDistrictCommand(cityDistrict.Id, cityDistrict.RowVersion, "NewName",
                "NewPolishName", Guid.NewGuid(), Guid.NewGuid(), new Collection<string> { "NewNameVariant" });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);
            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Parent_Is_Not_Found()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .Build();
            var getCityDistrict = GetResult<CityDistrict>.Ok(cityDistrict);
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.ParentNotFound, CityDistrictErrorMessage.ParentNotFound)
            };
            var parentExistsVerificationResult = VerificationResult.Fail(errors);
            var command = new UpdateCityDistrictCommand(cityDistrict.Id, cityDistrict.RowVersion, "NewName",
                "NewPolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NewNameVariant" });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);
            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _cityDistrictVerificationServiceMock.Setup(x => x.VerifyParentExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(parentExistsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Name_And_PolishName_Are_Already_Used()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .Build();
            var getCityDistrict = GetResult<CityDistrict>.Ok(cityDistrict);
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var parentExistsVerificationResult = VerificationResult.Ok();
            var nameIsNotTakenError = new Error(CityDistrictErrorCodeEnumeration.NameAlreadyInUse, CityDistrictErrorMessage.NameAlreadyInUse);
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { nameIsNotTakenError });
            var polishNameIsNotTakenError = new Error(CityDistrictErrorCodeEnumeration.PolishNameAlreadyInUse, CityDistrictErrorMessage.PolishNameAlreadyInUse);
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError> { polishNameIsNotTakenError });
            var command = new UpdateCityDistrictCommand(cityDistrict.Id, cityDistrict.RowVersion, "NewName",
                "NewPolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NewNameVariant" });
            var errors = new Collection<IError> { nameIsNotTakenError, polishNameIsNotTakenError };

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);
            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _cityDistrictVerificationServiceMock.Setup(x => x.VerifyParentExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(parentExistsVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Only_Name_Is_Already_Used()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .Build();
            var getCityDistrict = GetResult<CityDistrict>.Ok(cityDistrict);
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var parentExistsVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NameAlreadyInUse,
                    CityDistrictErrorMessage.NameAlreadyInUse)
            };
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(errors);
            var polishNameIsNotTakenVerificationResult = VerificationResult.Ok();
            var command = new UpdateCityDistrictCommand(cityDistrict.Id, cityDistrict.RowVersion, "NewName",
                "NewPolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NewNameVariant" });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);
            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _cityDistrictVerificationServiceMock.Setup(x => x.VerifyParentExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(parentExistsVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Only_PolishName_Is_Already_Used()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .Build();
            var getCityDistrict = GetResult<CityDistrict>.Ok(cityDistrict);
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var parentExistsVerificationResult = VerificationResult.Ok();
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.PolishNameAlreadyInUse,
                    CityDistrictErrorMessage.PolishNameAlreadyInUse)
            };
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(errors);
            var command = new UpdateCityDistrictCommand(cityDistrict.Id, cityDistrict.RowVersion, "NewName",
                "NewPolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NewNameVariant" });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrict);
            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _cityDistrictVerificationServiceMock.Setup(x => x.VerifyParentExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(parentExistsVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}