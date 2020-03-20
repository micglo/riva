using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmonaut;
using Moq;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.Announcements.Infrastructure.Test.DataAccessTests.RivaAnnouncementsCosmosDbTests.FlatForRentAnnouncementRepositoryTests
{
    public class FlatForRentAnnouncementRepositoryTestFixture
    {
        public IFlatForRentAnnouncementRepository Repository { get; }
        public ICosmosStore<FlatForRentAnnouncementEntity> CosmosStore { get; }
        public Mock<IMapper> MapperMock { get; }
        public Mock<IOrderByExpressionCreator<FlatForRentAnnouncementEntity>> OrderByExpressionCreatorMock { get; }
        public FlatForRentAnnouncement FlatForRentAnnouncement { get; }

        public FlatForRentAnnouncementRepositoryTestFixture(DatabaseFixture fixture)
        {
            CosmosStore = fixture.FlatForRentAnnouncementCosmosStore;
            MapperMock = new Mock<IMapper>();
            OrderByExpressionCreatorMock = new Mock<IOrderByExpressionCreator<FlatForRentAnnouncementEntity>>();
            Repository = new FlatForRentAnnouncementRepository(CosmosStore, MapperMock.Object, OrderByExpressionCreatorMock.Object);
            FlatForRentAnnouncement = InsertFlatForRentAnnouncementAsync().GetAwaiter().GetResult();
        }

        public async Task<FlatForRentAnnouncement> InsertFlatForRentAnnouncementAsync()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://sourceUrl")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(100)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var entity = new FlatForRentAnnouncementEntity
            {
                Id = flatForRentAnnouncement.Id,
                Title = flatForRentAnnouncement.Title,
                Created = flatForRentAnnouncement.Created,
                CityId = flatForRentAnnouncement.CityId,
                SourceUrl = flatForRentAnnouncement.SourceUrl,
                Description = flatForRentAnnouncement.Description,
                Price = flatForRentAnnouncement.Price,
                NumberOfRooms = flatForRentAnnouncement.NumberOfRooms.ConvertToEnum(),
                CityDistricts = flatForRentAnnouncement.CityDistricts,
                CosmosEntityName = nameof(Domain.FlatForRentAnnouncements.Aggregates.FlatForRentAnnouncement)
            };
            await CosmosStore.AddAsync(entity);
            return flatForRentAnnouncement;
        }
    }
}