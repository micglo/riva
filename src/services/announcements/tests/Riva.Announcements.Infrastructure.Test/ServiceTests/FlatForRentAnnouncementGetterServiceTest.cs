using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Announcements.Infrastructure.Test.ServiceTests
{
    public class FlatForRentAnnouncementGetterServiceTest
    {
        private readonly Mock<IFlatForRentAnnouncementRepository> _flatForRentAnnouncementRepositoryMock;
        private readonly IFlatForRentAnnouncementGetterService _service;

        public FlatForRentAnnouncementGetterServiceTest()
        {
            _flatForRentAnnouncementRepositoryMock = new Mock<IFlatForRentAnnouncementRepository>();
            _service = new FlatForRentAnnouncementGetterService(_flatForRentAnnouncementRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Ok()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://sourceUrl")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.FiveAndMore)
                .SetPrice(100)
                .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                .Build();
            var expectedResult = GetResult<FlatForRentAnnouncement>.Ok(flatForRentAnnouncement);

            _flatForRentAnnouncementRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(flatForRentAnnouncement);

            var result = await _service.GetByIdAsync(flatForRentAnnouncement.Id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Fail()
        {
            var errors = new Collection<IError>
            {
                new Error(FlatForRentAnnouncementErrorCodeEnumeration.NotFound, FlatForRentAnnouncementErrorMessage.NotFound)
            };
            var expectedResult = GetResult<FlatForRentAnnouncement>.Fail(errors);

            _flatForRentAnnouncementRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<FlatForRentAnnouncement>(null));

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}