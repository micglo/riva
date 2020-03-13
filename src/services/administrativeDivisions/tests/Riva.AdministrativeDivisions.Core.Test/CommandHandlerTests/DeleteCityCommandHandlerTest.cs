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
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.CommandHandlerTests
{
    public class DeleteCityCommandHandlerTest
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly Mock<ICityGetterService> _cityGetterServiceMock;
        private readonly ICommandHandler<DeleteCityCommand> _commandHandler;

        public DeleteCityCommandHandlerTest()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _cityGetterServiceMock = new Mock<ICityGetterService>();
            _commandHandler = new DeleteCityCommandHandler(_cityRepositoryMock.Object, _cityGetterServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_City()
        {
            var command = new DeleteCityCommand(Guid.NewGuid(), Array.Empty<byte>());
            var city = City.Builder()
                .SetId(command.CityId)
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _cityRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<City>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);

            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_City_Is_Not_Found()
        {
            var command = new DeleteCityCommand(Guid.NewGuid(), Array.Empty<byte>());
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var getCityResult = GetResult<City>.Fail(errors);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_PreconditionFailedException_When_RowVersion_Does_Not_Match()
        {
            var command = new DeleteCityCommand(Guid.NewGuid(), new byte[] { 1, 2, 4, 8, 16, 32, 64 });
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

            await result.Should().ThrowAsync<PreconditionFailedException>();
        }
    }
}