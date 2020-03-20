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
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.Announcements.Core.Test.QueryHandlerTests
{
    public class GetFlatForRentAnnouncementQueryHandlerTest
    {
        private readonly Mock<IFlatForRentAnnouncementGetterService> _flatForRentAnnouncementGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetFlatForRentAnnouncementInputQuery, FlatForRentAnnouncementOutputQuery> _queryHandler;

        public GetFlatForRentAnnouncementQueryHandlerTest()
        {
            _flatForRentAnnouncementGetterServiceMock = new Mock<IFlatForRentAnnouncementGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetFlatForRentAnnouncementQueryHandler(_flatForRentAnnouncementGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_FlatForRentAnnouncementOutputQuery()
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
            var flatForRentAnnouncementOutputQuery = new FlatForRentAnnouncementOutputQuery(flatForRentAnnouncement.Id,
                flatForRentAnnouncement.Title, flatForRentAnnouncement.SourceUrl, flatForRentAnnouncement.CityId,
                flatForRentAnnouncement.Created, flatForRentAnnouncement.Description, flatForRentAnnouncement.Price,
                flatForRentAnnouncement.NumberOfRooms, flatForRentAnnouncement.CityDistricts);

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);
            _mapperMock
                .Setup(x => x.Map<FlatForRentAnnouncement, FlatForRentAnnouncementOutputQuery>(It.IsAny<FlatForRentAnnouncement>()))
                .Returns(flatForRentAnnouncementOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetFlatForRentAnnouncementInputQuery(flatForRentAnnouncement.Id));

            result.Should().BeEquivalentTo(flatForRentAnnouncementOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_FlatForRentAnnouncement_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(FlatForRentAnnouncementErrorCodeEnumeration.NotFound, FlatForRentAnnouncementErrorMessage.NotFound)
            };
            var getFlatForRentAnnouncementResult = GetResult<FlatForRentAnnouncement>.Fail(errors);

            _flatForRentAnnouncementGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getFlatForRentAnnouncementResult);

            Func<Task<FlatForRentAnnouncementOutputQuery>> result = async () => await _queryHandler.HandleAsync(new GetFlatForRentAnnouncementInputQuery(Guid.NewGuid()));
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}