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
    public class DeleteFlatForRentAnnouncementCommandHandlerTest
    {
        private readonly Mock<IFlatForRentAnnouncementGetterService> _flatForRentAnnouncementGetterServiceMock;
        private readonly Mock<IFlatForRentAnnouncementRepository> _flatForRentAnnouncementRepositoryMock;
        private readonly ICommandHandler<DeleteFlatForRentAnnouncementCommand> _commandHandler;

        public DeleteFlatForRentAnnouncementCommandHandlerTest()
        {
            _flatForRentAnnouncementGetterServiceMock = new Mock<IFlatForRentAnnouncementGetterService>();
            _flatForRentAnnouncementRepositoryMock = new Mock<IFlatForRentAnnouncementRepository>();
            _commandHandler = new DeleteFlatForRentAnnouncementCommandHandler(
                _flatForRentAnnouncementGetterServiceMock.Object, _flatForRentAnnouncementRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_FlatForRentAnnouncement()
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
            var deleteFlatForRentAnnouncementCommand = new DeleteFlatForRentAnnouncementCommand(flatForRentAnnouncement.Id);

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);
            _flatForRentAnnouncementRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<FlatForRentAnnouncement>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteFlatForRentAnnouncementCommand);
            
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_FlatForRentAnnouncement_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(FlatForRentAnnouncementErrorCodeEnumeration.NotFound, FlatForRentAnnouncementErrorMessage.NotFound)
            };
            var getFlatForRentAnnouncementResult = GetResult<FlatForRentAnnouncement>.Fail(errors);
            var deleteFlatForRentAnnouncementCommand = new DeleteFlatForRentAnnouncementCommand(Guid.NewGuid());

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteFlatForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}