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
    public class DeleteRoomForRentAnnouncementCommandHandlerTest
    {
        private readonly Mock<IRoomForRentAnnouncementGetterService> _roomForRentAnnouncementGetterServiceMock;
        private readonly Mock<IRoomForRentAnnouncementRepository> _roomForRentAnnouncementRepositoryMock;
        private readonly ICommandHandler<DeleteRoomForRentAnnouncementCommand> _commandHandler;

        public DeleteRoomForRentAnnouncementCommandHandlerTest()
        {
            _roomForRentAnnouncementGetterServiceMock = new Mock<IRoomForRentAnnouncementGetterService>();
            _roomForRentAnnouncementRepositoryMock = new Mock<IRoomForRentAnnouncementRepository>();
            _commandHandler = new DeleteRoomForRentAnnouncementCommandHandler(
                _roomForRentAnnouncementGetterServiceMock.Object, _roomForRentAnnouncementRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_RoomForRentAnnouncement()
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
                .SetRoomTypes(new List<RoomTypeEnumeration>{RoomTypeEnumeration.Double})
                .Build();
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Ok(roomForRentAnnouncement);
            var deleteRoomForRentAnnouncementCommand = new DeleteRoomForRentAnnouncementCommand(roomForRentAnnouncement.Id);

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);
            _roomForRentAnnouncementRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<RoomForRentAnnouncement>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteRoomForRentAnnouncementCommand);
            
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_RoomForRentAnnouncement_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(RoomForRentAnnouncementErrorCodeEnumeration.NotFound, RoomForRentAnnouncementErrorMessage.NotFound)
            };
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Fail(errors);
            var deleteRoomForRentAnnouncementCommand = new DeleteRoomForRentAnnouncementCommand(Guid.NewGuid());

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteRoomForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}