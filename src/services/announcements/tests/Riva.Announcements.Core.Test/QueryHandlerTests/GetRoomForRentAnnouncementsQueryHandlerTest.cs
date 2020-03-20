using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Core.Queries.Handlers;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.Announcements.Core.Test.QueryHandlerTests
{
    public class GetRoomForRentAnnouncementsQueryHandlerTest
    {
        private readonly Mock<IRoomForRentAnnouncementRepository> _roomForRentAnnouncementRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetRoomForRentAnnouncementsInputQuery, CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>> _queryHandler;

        public GetRoomForRentAnnouncementsQueryHandlerTest()
        {
            _roomForRentAnnouncementRepositoryMock = new Mock<IRoomForRentAnnouncementRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetRoomForRentAnnouncementsQueryHandler(_roomForRentAnnouncementRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_RoomForRentAnnouncementOutputQuery_When_Input_Is_Not_Null()
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
                .SetRoomTypes(new []{RoomTypeEnumeration.Double})
                .Build();
            var roomForRentAnnouncements = new List<RoomForRentAnnouncement> { roomForRentAnnouncement };
            var roomForRentAnnouncementOutputQueries = roomForRentAnnouncements.Select(x =>
                new RoomForRentAnnouncementOutputQuery(x.Id, x.Title, x.SourceUrl, x.CityId, x.Created, x.Description, x.Price, x.RoomTypes, x.CityDistricts)).ToList();
            var roomForRentAnnouncementsInputQuery =
                new GetRoomForRentAnnouncementsInputQuery(1, 100, "price:asc", null, null, null, null, null, null, null);
            var collectionOutputQuery =
                new CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>(roomForRentAnnouncementOutputQueries.Count, roomForRentAnnouncementOutputQueries);

            _roomForRentAnnouncementRepositoryMock
                .Setup(x => x.FindAsync(It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(),
                    It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<Guid?>(), It.IsAny<RoomTypeEnumeration>()))
                .ReturnsAsync(roomForRentAnnouncements);
            _roomForRentAnnouncementRepositoryMock
                .Setup(x => x.CountAsync(It.IsAny<Guid?>(), It.IsAny<DateTimeOffset?>(), 
                    It.IsAny<DateTimeOffset?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<Guid?>(), 
                    It.IsAny<RoomTypeEnumeration>()))
                .ReturnsAsync(roomForRentAnnouncements.Count);
            _mapperMock
                .Setup(x => x.Map<List<RoomForRentAnnouncement>, IEnumerable<RoomForRentAnnouncementOutputQuery>>(It.IsAny<List<RoomForRentAnnouncement>>()))
                .Returns(roomForRentAnnouncementOutputQueries);

            var result = await _queryHandler.HandleAsync(roomForRentAnnouncementsInputQuery);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_RoomForRentAnnouncementOutputQuery_When_Input_Is_Null()
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
                .SetRoomTypes(new[] { RoomTypeEnumeration.Double })
                .Build();
            var roomForRentAnnouncements = new List<RoomForRentAnnouncement> { roomForRentAnnouncement };
            var roomForRentAnnouncementOutputQueries = roomForRentAnnouncements.Select(x =>
                new RoomForRentAnnouncementOutputQuery(x.Id, x.Title, x.SourceUrl, x.CityId, x.Created, x.Description, x.Price, x.RoomTypes, x.CityDistricts)).ToList();
            var collectionOutputQuery =
                new CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>(roomForRentAnnouncementOutputQueries.Count, roomForRentAnnouncementOutputQueries);

            _roomForRentAnnouncementRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(roomForRentAnnouncements);
            _roomForRentAnnouncementRepositoryMock.Setup(x => x.CountAsync())
                .ReturnsAsync(roomForRentAnnouncements.Count);
            _mapperMock
                .Setup(x => x.Map<List<RoomForRentAnnouncement>, IEnumerable<RoomForRentAnnouncementOutputQuery>>(It.IsAny<List<RoomForRentAnnouncement>>()))
                .Returns(roomForRentAnnouncementOutputQueries);

            var result = await _queryHandler.HandleAsync(null);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }
    }
}