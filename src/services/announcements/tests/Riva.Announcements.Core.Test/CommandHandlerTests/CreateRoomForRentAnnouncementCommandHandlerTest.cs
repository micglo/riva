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
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Announcements.Core.Test.CommandHandlerTests
{
    public class CreateRoomForRentAnnouncementCommandHandlerTest
    {
        private readonly Mock<IRoomForRentAnnouncementRepository> _roomForRentAnnouncementRepositoryMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<ICityDistrictVerificationService> _cityDistrictVerificationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICommandHandler<CreateRoomForRentAnnouncementCommand> _commandHandler;

        public CreateRoomForRentAnnouncementCommandHandlerTest()
        {
            _roomForRentAnnouncementRepositoryMock = new Mock<IRoomForRentAnnouncementRepository>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _cityDistrictVerificationServiceMock = new Mock<ICityDistrictVerificationService>();
            _mapperMock = new Mock<IMapper>();
            _commandHandler = new CreateRoomForRentAnnouncementCommandHandler(
                _roomForRentAnnouncementRepositoryMock.Object, _cityVerificationServiceMock.Object,
                _cityDistrictVerificationServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_RoomForRentAnnouncement()
        {
            var cityVerificationResult = VerificationResult.Ok();
            var cityDistrictsVerificationResult = VerificationResult.Ok();
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration>{RoomTypeEnumeration.Single})
                .Build();
            var createRoomForRentAnnouncementCommand = new CreateRoomForRentAnnouncementCommand(
                roomForRentAnnouncement.Id, 
                roomForRentAnnouncement.Title, 
                roomForRentAnnouncement.SourceUrl,
                roomForRentAnnouncement.CityId, 
                roomForRentAnnouncement.Description, 
                roomForRentAnnouncement.Price,
                roomForRentAnnouncement.RoomTypes,
                roomForRentAnnouncement.CityDistricts);

            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);
            _mapperMock
                .Setup(x => x.Map<CreateRoomForRentAnnouncementCommand, RoomForRentAnnouncement>(It.IsAny<CreateRoomForRentAnnouncementCommand>()))
                .Returns(roomForRentAnnouncement);
            _roomForRentAnnouncementRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<RoomForRentAnnouncement>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createRoomForRentAnnouncementCommand);

            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var cityVerificationResult = VerificationResult.Fail(errors);
            var createRoomForRentAnnouncementCommand = new CreateRoomForRentAnnouncementCommand(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), "Description", null, 
                new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single }, new List<Guid>());

            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createRoomForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Any_Of_CityDistricts_Is_Not_Found()
        {
            var cityVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var cityDistrictsVerificationResult = VerificationResult.Fail(errors);
            var createRoomForRentAnnouncementCommand = new CreateRoomForRentAnnouncementCommand(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), "Description", null, 
                new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single }, new List<Guid>());

            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createRoomForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}