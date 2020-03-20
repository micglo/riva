using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Commands.Handlers;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Announcements.Core.Test.CommandHandlerTests
{
    public class UpdateFlatForRentAnnouncementCommandHandlerTest
    {
        private readonly Mock<IFlatForRentAnnouncementGetterService> _flatForRentAnnouncementGetterServiceMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<ICityDistrictVerificationService> _cityDistrictVerificationServiceMock;
        private readonly Mock<IFlatForRentAnnouncementRepository> _flatForRentAnnouncementRepositoryMock;
        private readonly ICommandHandler<UpdateFlatForRentAnnouncementCommand> _commandHandler;

        public UpdateFlatForRentAnnouncementCommandHandlerTest()
        {
            _flatForRentAnnouncementGetterServiceMock = new Mock<IFlatForRentAnnouncementGetterService>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _cityDistrictVerificationServiceMock = new Mock<ICityDistrictVerificationService>();
            _flatForRentAnnouncementRepositoryMock = new Mock<IFlatForRentAnnouncementRepository>();
            _commandHandler = new UpdateFlatForRentAnnouncementCommandHandler(
                _flatForRentAnnouncementGetterServiceMock.Object, _cityVerificationServiceMock.Object,
                _cityDistrictVerificationServiceMock.Object, _flatForRentAnnouncementRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_FlatForRentAnnouncement()
        {
            var cityDistrictToRemove = Guid.NewGuid();
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { cityDistrictToRemove })
                .Build();
            var getFlatForRentAnnouncementResult = GetResult<FlatForRentAnnouncement>.Ok(flatForRentAnnouncement);
            var cityVerificationResult = VerificationResult.Ok();
            var cityDistrictsVerificationResult = VerificationResult.Ok();
            var cityDistrictToAdd = Guid.NewGuid();
            const string newTitle = "NewTitle";
            var updateFlatForRentAnnouncementCommand = new UpdateFlatForRentAnnouncementCommand(
                flatForRentAnnouncement.Id, newTitle, flatForRentAnnouncement.SourceUrl,
                flatForRentAnnouncement.CityId, flatForRentAnnouncement.Description, flatForRentAnnouncement.Price,
                flatForRentAnnouncement.NumberOfRooms, new List<Guid>{ cityDistrictToAdd });

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);
            _flatForRentAnnouncementRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<FlatForRentAnnouncement>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateFlatForRentAnnouncementCommand);

            await result.Should().NotThrowAsync<Exception>();
            flatForRentAnnouncement.Title.Should().Be(newTitle);
            flatForRentAnnouncement.CityDistricts.Should().NotContain(cityDistrictToRemove);
            flatForRentAnnouncement.CityDistricts.Should().Contain(cityDistrictToAdd);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_FlatForRentAnnouncement_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(FlatForRentAnnouncementErrorCodeEnumeration.NotFound, FlatForRentAnnouncementErrorMessage.NotFound)
            };
            var getFlatForRentAnnouncementResult = GetResult<FlatForRentAnnouncement>.Fail(errors);
            var updateFlatForRentAnnouncementCommand = new UpdateFlatForRentAnnouncementCommand(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), "Description", null, null, new List<Guid>());

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateFlatForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_Is_Not_Found()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementResult = GetResult<FlatForRentAnnouncement>.Ok(flatForRentAnnouncement);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var cityVerificationResult = VerificationResult.Fail(errors);
            var updateFlatForRentAnnouncementCommand = new UpdateFlatForRentAnnouncementCommand(
                flatForRentAnnouncement.Id, flatForRentAnnouncement.Title, flatForRentAnnouncement.SourceUrl,
                flatForRentAnnouncement.CityId, flatForRentAnnouncement.Description, flatForRentAnnouncement.Price,
                flatForRentAnnouncement.NumberOfRooms, flatForRentAnnouncement.CityDistricts);

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateFlatForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Any_Of_CityDistricts_Is_Not_Found()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementResult = GetResult<FlatForRentAnnouncement>.Ok(flatForRentAnnouncement);
            var cityVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var cityDistrictsVerificationResult = VerificationResult.Fail(errors);
            var updateFlatForRentAnnouncementCommand = new UpdateFlatForRentAnnouncementCommand(
                flatForRentAnnouncement.Id, flatForRentAnnouncement.Title, flatForRentAnnouncement.SourceUrl,
                flatForRentAnnouncement.CityId, flatForRentAnnouncement.Description, flatForRentAnnouncement.Price,
                flatForRentAnnouncement.NumberOfRooms, flatForRentAnnouncement.CityDistricts);

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateFlatForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}