using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Moq;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.Announcements.Infrastructure.Test.DataAccessTests.RivaAnnouncementsCosmosDbTests.RoomForRentAnnouncementRepositoryTests
{
    public class RoomForRentAnnouncementRepositoryTestFixture
    {
        public IRoomForRentAnnouncementRepository Repository { get; }
        public ICosmosStore<RoomForRentAnnouncementEntity> CosmosStore { get; }
        public Mock<IMapper> MapperMock { get; }
        public Mock<IOrderByExpressionCreator<RoomForRentAnnouncementEntity>> OrderByExpressionCreatorMock { get; }
        public RoomForRentAnnouncement RoomForRentAnnouncement { get; }

        public RoomForRentAnnouncementRepositoryTestFixture(DatabaseFixture fixture)
        {
            CosmosStore = fixture.RoomForRentAnnouncementCosmosStore;
            MapperMock = new Mock<IMapper>();
            OrderByExpressionCreatorMock = new Mock<IOrderByExpressionCreator<RoomForRentAnnouncementEntity>>();
            Repository = new RoomForRentAnnouncementRepository(CosmosStore, MapperMock.Object, OrderByExpressionCreatorMock.Object);
            RoomForRentAnnouncement = InsertRoomForRentAnnouncementAsync().GetAwaiter().GetResult();
        }

        public async Task<RoomForRentAnnouncement> InsertRoomForRentAnnouncementAsync()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://sourceUrl")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(100)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.MultiPerson })
                .Build();
            var entity = new RoomForRentAnnouncementEntity
            {
                Id = roomForRentAnnouncement.Id,
                Title = roomForRentAnnouncement.Title,
                Created = roomForRentAnnouncement.Created,
                CityId = roomForRentAnnouncement.CityId,
                SourceUrl = roomForRentAnnouncement.SourceUrl,
                Description = roomForRentAnnouncement.Description,
                Price = roomForRentAnnouncement.Price,
                CityDistricts = roomForRentAnnouncement.CityDistricts,
                RoomTypes = roomForRentAnnouncement.RoomTypes.Select(x => x.ConvertToEnum()),
                CosmosEntityName = nameof(Domain.RoomForRentAnnouncements.Aggregates.RoomForRentAnnouncement)
            };
            await CosmosStore.AddAsync(entity);
            return roomForRentAnnouncement;
        }
    }
}