using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Announcements.Infrastructure.Test.ServiceTests
{
    public class RoomForRentAnnouncementGetterServiceTest
    {
        private readonly Mock<IRoomForRentAnnouncementRepository> _roomForRentAnnouncementRepositoryMock;
        private readonly IRoomForRentAnnouncementGetterService _service;

        public RoomForRentAnnouncementGetterServiceTest()
        {
            _roomForRentAnnouncementRepositoryMock = new Mock<IRoomForRentAnnouncementRepository>();
            _service = new RoomForRentAnnouncementGetterService(_roomForRentAnnouncementRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Ok()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://sourceUrl")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetPrice(100)
                .SetRoomTypes(new List<RoomTypeEnumeration>{RoomTypeEnumeration.Double})
                .Build();
            var expectedResult = GetResult<RoomForRentAnnouncement>.Ok(roomForRentAnnouncement);

            _roomForRentAnnouncementRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(roomForRentAnnouncement);

            var result = await _service.GetByIdAsync(roomForRentAnnouncement.Id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Fail()
        {
            var errors = new Collection<IError>
            {
                new Error(RoomForRentAnnouncementErrorCodeEnumeration.NotFound, RoomForRentAnnouncementErrorMessage.NotFound)
            };
            var expectedResult = GetResult<RoomForRentAnnouncement>.Fail(errors);

            _roomForRentAnnouncementRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<RoomForRentAnnouncement>(null));

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}