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
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class CreateCityDistrictCommandHandlerTest
    {
        private readonly Mock<ICityGetterService> _cityGetterServiceMock;
        private readonly Mock<ICityDistrictVerificationService> _cityDistrictVerificationServiceMock;
        private readonly Mock<ICityDistrictRepository> _cityDistrictRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICommandHandler<CreateCityDistrictCommand> _commandHandler;

        public CreateCityDistrictCommandHandlerTest()
        {
            _cityGetterServiceMock = new Mock<ICityGetterService>();
            _cityDistrictVerificationServiceMock = new Mock<ICityDistrictVerificationService>();
            _cityDistrictRepositoryMock = new Mock<ICityDistrictRepository>();
            _mapperMock = new Mock<IMapper>();
            _commandHandler = new CreateCityDistrictCommandHandler(_cityGetterServiceMock.Object,
                _cityDistrictVerificationServiceMock.Object, _cityDistrictRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_CityDistrict()
        {
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
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(city.StateId)
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string>{ "NameVariant" })
                .Build();
            var command = new CreateCityDistrictCommand(cityDistrict.Id, cityDistrict.Name,
                cityDistrict.PolishName, cityDistrict.CityId, cityDistrict.ParentId, cityDistrict.NameVariants);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _cityDistrictVerificationServiceMock.Setup(x => x.VerifyParentExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(parentExistsVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyPolishNameIsNotTakenAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(polishNameIsNotTakenVerificationResult);
            _mapperMock.Setup(
                    x => x.Map<CreateCityDistrictCommand, CityDistrict>(It.IsAny<CreateCityDistrictCommand>()))
                .Returns(cityDistrict);
            _cityDistrictRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CityDistrict>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var getCityResult = GetResult<City>.Fail(errors);
            var command = new CreateCityDistrictCommand(Guid.NewGuid(),  "Name",
                "PolishName", Guid.NewGuid(), Guid.NewGuid(), new Collection<string>{ "NameVariant" });

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Parent_Is_Not_Found()
        {
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
            var command = new CreateCityDistrictCommand(Guid.NewGuid(), "Name",
                "PolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NameVariant" });

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
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError>{nameIsNotTakenError});
            var polishNameIsNotTakenError = new Error(CityDistrictErrorCodeEnumeration.PolishNameAlreadyInUse, CityDistrictErrorMessage.PolishNameAlreadyInUse);
            var polishNameIsNotTakenVerificationResult = VerificationResult.Fail(new Collection<IError>{polishNameIsNotTakenError});
            var command = new CreateCityDistrictCommand(Guid.NewGuid(), "Name",
                "PolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NameVariant" });
            var errors = new Collection<IError> { nameIsNotTakenError, polishNameIsNotTakenError };

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
            var command = new CreateCityDistrictCommand(Guid.NewGuid(), "Name",
                "PolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NameVariant" });

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
            var command = new CreateCityDistrictCommand(Guid.NewGuid(), "Name",
                "PolishName", city.Id, Guid.NewGuid(), new Collection<string> { "NameVariant" });

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