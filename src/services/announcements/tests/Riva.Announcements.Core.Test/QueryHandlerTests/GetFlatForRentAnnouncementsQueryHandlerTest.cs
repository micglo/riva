using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Core.Queries.Handlers;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.Announcements.Core.Test.QueryHandlerTests
{
    public class GetFlatForRentAnnouncementsQueryHandlerTest
    {
        private readonly Mock<IFlatForRentAnnouncementRepository> _flatForRentAnnouncementRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetFlatForRentAnnouncementsInputQuery, CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>> _queryHandler;

        public GetFlatForRentAnnouncementsQueryHandlerTest()
        {
            _flatForRentAnnouncementRepositoryMock = new Mock<IFlatForRentAnnouncementRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetFlatForRentAnnouncementsQueryHandler(_flatForRentAnnouncementRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_FlatForRentAnnouncementOutputQuery_When_Input_Is_Not_Null()
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
            var flatForRentAnnouncements = new List<FlatForRentAnnouncement> { flatForRentAnnouncement };
            var forRentAnnouncementOutputQueries = flatForRentAnnouncements.Select(x =>
                new FlatForRentAnnouncementOutputQuery(x.Id, x.Title, x.SourceUrl, x.CityId, x.Created, x.Description,
                    x.Price, x.NumberOfRooms, x.CityDistricts)).ToList();
            var flatForRentAnnouncementsInputQuery =
                new GetFlatForRentAnnouncementsInputQuery(1, 100, "price:asc", null, null, null, null, null, null,
                    null);
            var collectionOutputQuery = new CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>(forRentAnnouncementOutputQueries.Count,
                    forRentAnnouncementOutputQueries);

            _flatForRentAnnouncementRepositoryMock
                .Setup(x => x.FindAsync(It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(),
                    It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<Guid?>(), It.IsAny<NumberOfRoomsEnumeration>()))
                .ReturnsAsync(flatForRentAnnouncements);
            _flatForRentAnnouncementRepositoryMock.Setup(x => x.CountAsync(It.IsAny<Guid?>(),
                It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(),
                It.IsAny<Guid?>(), It.IsAny<NumberOfRoomsEnumeration>())).ReturnsAsync(flatForRentAnnouncements.Count);
            _mapperMock
                .Setup(x => x.Map<List<FlatForRentAnnouncement>, IEnumerable<FlatForRentAnnouncementOutputQuery>>(It.IsAny<List<FlatForRentAnnouncement>>()))
                .Returns(forRentAnnouncementOutputQueries);

            var result = await _queryHandler.HandleAsync(flatForRentAnnouncementsInputQuery);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_FlatForRentAnnouncementOutputQuery_When_Input_Is_Null()
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
            var flatForRentAnnouncements = new List<FlatForRentAnnouncement> { flatForRentAnnouncement };
            var forRentAnnouncementOutputQueries = flatForRentAnnouncements
                .Select(x => new FlatForRentAnnouncementOutputQuery(x.Id, x.Title, x.SourceUrl, x.CityId, x.Created, x.Description,
                    x.Price, x.NumberOfRooms, x.CityDistricts)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>(forRentAnnouncementOutputQueries.Count,
                    forRentAnnouncementOutputQueries);

            _flatForRentAnnouncementRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(flatForRentAnnouncements);
            _flatForRentAnnouncementRepositoryMock.Setup(x => x.CountAsync())
                .ReturnsAsync(flatForRentAnnouncements.Count);
            _mapperMock
                .Setup(x => x.Map<List<FlatForRentAnnouncement>, IEnumerable<FlatForRentAnnouncementOutputQuery>>(It.IsAny<List<FlatForRentAnnouncement>>()))
                .Returns(forRentAnnouncementOutputQueries);

            var result = await _queryHandler.HandleAsync(null);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }
    }
}