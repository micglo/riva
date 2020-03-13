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
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class DeleteCityDistrictCommandHandlerTest
    {
        private readonly Mock<ICityDistrictGetterService> _cityDistrictGetterServiceMock;
        private readonly Mock<ICityDistrictRepository> _cityDistrictRepositoryMock;
        private readonly ICommandHandler<DeleteCityDistrictCommand> _commandHandler;

        public DeleteCityDistrictCommandHandlerTest()
        {
            _cityDistrictGetterServiceMock = new Mock<ICityDistrictGetterService>();
            _cityDistrictRepositoryMock = new Mock<ICityDistrictRepository>();
            _commandHandler = new DeleteCityDistrictCommandHandler(_cityDistrictGetterServiceMock.Object,
                _cityDistrictRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_CityDistrict()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();
            var getCityDistrictResult = GetResult<CityDistrict>.Ok(cityDistrict);
            var command = new DeleteCityDistrictCommand(cityDistrict.Id, cityDistrict.RowVersion);

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrictResult);
            _cityDistrictRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<CityDistrict>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When__CityDistrict_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var getCityDistrictResult = GetResult<CityDistrict>.Fail(errors);
            var command = new DeleteCityDistrictCommand(Guid.NewGuid(), Array.Empty<byte>());

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrictResult);
            _cityDistrictRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<CityDistrict>())).Returns(Task.CompletedTask);

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
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();
            var getCityDistrictResult = GetResult<CityDistrict>.Ok(cityDistrict);
            var command = new DeleteCityDistrictCommand(cityDistrict.Id, new byte[] { 1, 2, 4, 8, 16, 32, 128 });

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrictResult);
            _cityDistrictRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<CityDistrict>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().ThrowExactlyAsync<PreconditionFailedException>();
        }
    }
}