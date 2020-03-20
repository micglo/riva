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
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Announcements.Core.Test.CommandHandlerTests
{
    public class UpdateRoomForRentAnnouncementCommandHandlerTest
    {
        private readonly Mock<IRoomForRentAnnouncementGetterService> _roomForRentAnnouncementGetterServiceMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<ICityDistrictVerificationService> _cityDistrictVerificationServiceMock;
        private readonly Mock<IRoomForRentAnnouncementRepository> _roomForRentAnnouncementRepositoryMock;
        private readonly ICommandHandler<UpdateRoomForRentAnnouncementCommand> _commandHandler;

        public UpdateRoomForRentAnnouncementCommandHandlerTest()
        {
            _roomForRentAnnouncementGetterServiceMock = new Mock<IRoomForRentAnnouncementGetterService>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _cityDistrictVerificationServiceMock = new Mock<ICityDistrictVerificationService>();
            _roomForRentAnnouncementRepositoryMock = new Mock<IRoomForRentAnnouncementRepository>();
            _commandHandler = new UpdateRoomForRentAnnouncementCommandHandler(
                _roomForRentAnnouncementGetterServiceMock.Object, _cityVerificationServiceMock.Object,
                _cityDistrictVerificationServiceMock.Object, _roomForRentAnnouncementRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_RoomForRentAnnouncement()
        {
            var cityDistrictToRemove = Guid.NewGuid();
            var roomTypeToRemove = RoomTypeEnumeration.MultiPerson;
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { cityDistrictToRemove })
                .SetRoomTypes(new List<RoomTypeEnumeration>{ roomTypeToRemove })
                .Build();
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Ok(roomForRentAnnouncement);
            var cityVerificationResult = VerificationResult.Ok();
            var cityDistrictsVerificationResult = VerificationResult.Ok();
            const string newTitle = "NewTitle";
            var cityDistrictToAdd = Guid.NewGuid();
            var roomTypeToAdd = RoomTypeEnumeration.Single;
            var updateRoomForRentAnnouncementCommand = new UpdateRoomForRentAnnouncementCommand(
                roomForRentAnnouncement.Id, newTitle, roomForRentAnnouncement.SourceUrl,
                roomForRentAnnouncement.CityId, roomForRentAnnouncement.Description, roomForRentAnnouncement.Price,
                new List<RoomTypeEnumeration>{ roomTypeToAdd }, new List<Guid>{ cityDistrictToAdd });

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);
            _roomForRentAnnouncementRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<RoomForRentAnnouncement>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoomForRentAnnouncementCommand);

            await result.Should().NotThrowAsync<Exception>();
            roomForRentAnnouncement.Title.Should().Be(newTitle);
            roomForRentAnnouncement.CityDistricts.Should().NotContain(cityDistrictToRemove);
            roomForRentAnnouncement.CityDistricts.Should().Contain(cityDistrictToAdd);
            roomForRentAnnouncement.RoomTypes.Should().NotContain(roomTypeToRemove);
            roomForRentAnnouncement.RoomTypes.Should().Contain(roomTypeToAdd);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_RoomForRentAnnouncement_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(RoomForRentAnnouncementErrorCodeEnumeration.NotFound, RoomForRentAnnouncementErrorMessage.NotFound)
            };
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Fail(errors);
            var updateRoomForRentAnnouncementCommand = new UpdateRoomForRentAnnouncementCommand(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), "Description", null,
                new List<RoomTypeEnumeration>(), new List<Guid>());

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoomForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_Is_Not_Found()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Ok(roomForRentAnnouncement);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var cityVerificationResult = VerificationResult.Fail(errors);
            var updateRoomForRentAnnouncementCommand = new UpdateRoomForRentAnnouncementCommand(
                roomForRentAnnouncement.Id, roomForRentAnnouncement.Title, roomForRentAnnouncement.SourceUrl,
                roomForRentAnnouncement.CityId, roomForRentAnnouncement.Description, roomForRentAnnouncement.Price,
                roomForRentAnnouncement.RoomTypes, roomForRentAnnouncement.CityDistricts);

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoomForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Any_Of_CityDistricts_Is_Not_Found()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Ok(roomForRentAnnouncement);
            var cityVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var cityDistrictsVerificationResult = VerificationResult.Fail(errors);
            var updateRoomForRentAnnouncementCommand = new UpdateRoomForRentAnnouncementCommand(
                roomForRentAnnouncement.Id, roomForRentAnnouncement.Title, roomForRentAnnouncement.SourceUrl,
                roomForRentAnnouncement.CityId, roomForRentAnnouncement.Description, roomForRentAnnouncement.Price,
                roomForRentAnnouncement.RoomTypes, roomForRentAnnouncement.CityDistricts);

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);
            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoomForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}