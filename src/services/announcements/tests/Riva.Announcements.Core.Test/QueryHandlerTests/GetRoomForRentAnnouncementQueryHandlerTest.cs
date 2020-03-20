using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Core.Queries.Handlers;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.Announcements.Core.Test.QueryHandlerTests
{
    public class GetRoomForRentAnnouncementQueryHandlerTest
    {
        private readonly Mock<IRoomForRentAnnouncementGetterService> _roomForRentAnnouncementGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetRoomForRentAnnouncementInputQuery, RoomForRentAnnouncementOutputQuery> _queryHandler;

        public GetRoomForRentAnnouncementQueryHandlerTest()
        {
            _roomForRentAnnouncementGetterServiceMock = new Mock<IRoomForRentAnnouncementGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetRoomForRentAnnouncementQueryHandler(_roomForRentAnnouncementGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_RoomForRentAnnouncementOutputQuery()
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
                .SetRoomTypes(new List<RoomTypeEnumeration>{ RoomTypeEnumeration.Double })
                .Build();
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Ok(roomForRentAnnouncement);
            var roomForRentAnnouncementOutputQuery = new RoomForRentAnnouncementOutputQuery(roomForRentAnnouncement.Id,
                roomForRentAnnouncement.Title, roomForRentAnnouncement.SourceUrl, roomForRentAnnouncement.CityId,
                roomForRentAnnouncement.Created, roomForRentAnnouncement.Description, roomForRentAnnouncement.Price,
                roomForRentAnnouncement.RoomTypes, roomForRentAnnouncement.CityDistricts);

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);
            _mapperMock
                .Setup(x => x.Map<RoomForRentAnnouncement, RoomForRentAnnouncementOutputQuery>(It.IsAny<RoomForRentAnnouncement>()))
                .Returns(roomForRentAnnouncementOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetRoomForRentAnnouncementInputQuery(roomForRentAnnouncement.Id));

            result.Should().BeEquivalentTo(roomForRentAnnouncementOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_RoomForRentAnnouncement_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(RoomForRentAnnouncementErrorCodeEnumeration.NotFound, RoomForRentAnnouncementErrorMessage.NotFound)
            };
            var getRoomForRentAnnouncementResult = GetResult<RoomForRentAnnouncement>.Fail(errors);

            _roomForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getRoomForRentAnnouncementResult);

            Func<Task<RoomForRentAnnouncementOutputQuery>> result = async () => await _queryHandler.HandleAsync(new GetRoomForRentAnnouncementInputQuery(Guid.NewGuid()));
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}