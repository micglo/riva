using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using FluentAssertions;
using Moq;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Xunit;

namespace Riva.Announcements.Infrastructure.Test.DataAccessTests.RivaAnnouncementsCosmosDbTests.RoomForRentAnnouncementRepositoryTests                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
{
    [Collection("RivaAnnouncementsCosmosDb tests collection")]
    public class RoomForRentAnnouncementRepositoryTest : IClassFixture<RoomForRentAnnouncementRepositoryTestFixture>
    {
        private readonly IRoomForRentAnnouncementRepository _repository;
        private readonly ICosmosStore<RoomForRentAnnouncementEntity> _cosmosStore;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderByExpressionCreator<RoomForRentAnnouncementEntity>> _orderByExpressionCreatorMock;
        private readonly RoomForRentAnnouncement _roomForRentAnnouncement;
        private readonly RoomForRentAnnouncementRepositoryTestFixture _fixture;

        public RoomForRentAnnouncementRepositoryTest(RoomForRentAnnouncementRepositoryTestFixture fixture)
        {
            _cosmosStore = fixture.CosmosStore;
            _mapperMock = fixture.MapperMock;
            _orderByExpressionCreatorMock = fixture.OrderByExpressionCreatorMock;
            _repository = fixture.Repository;
            _roomForRentAnnouncement = fixture.RoomForRentAnnouncement;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_RoomForRentAnnouncements()
        {
            var roomForRentAnnouncementEntities = await _cosmosStore.Query().ToListAsync();
            var roomForRentAnnouncements = roomForRentAnnouncementEntities
                .Select(x => RoomForRentAnnouncement.Builder()
                    .SetId(x.Id)
                    .SetTitle(x.Title)
                    .SetSourceUrl(x.SourceUrl)
                    .SetCityId(x.CityId)
                    .SetCreated(x.Created)
                    .SetDescription(x.Description)
                    .SetPrice(x.Price)
                    .SetCityDistricts(x.CityDistricts)
                    .SetRoomTypes(x.RoomTypes.Select(r => r.ConvertToEnumeration()))
                    .Build()
                )
                .ToList();

            _mapperMock
                .Setup(x => x.Map<List<RoomForRentAnnouncementEntity>, List<RoomForRentAnnouncement>>(It.IsAny<List<RoomForRentAnnouncementEntity>>()))
                .Returns(roomForRentAnnouncements);
            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(roomForRentAnnouncements);
        }

        [Fact]
        public async Task FindAsync_Should_Return_RoomForRentAnnouncements()
        {
            const int page = 1;
            const int pageSize = 3;
            const string sort = "created:desc";
            static IOrderedQueryable<RoomForRentAnnouncementEntity> OrderByExpression(IQueryable<RoomForRentAnnouncementEntity> o) => o.OrderByDescending(x => x.Created);
            var cityDistrict = _roomForRentAnnouncement.CityDistricts.FirstOrDefault();
            var roomType = _roomForRentAnnouncement.RoomTypes.FirstOrDefault();

            var roomForRentAnnouncementEntities = await _cosmosStore.Query()
                .OrderByDescending(x => x.Created)
                .Where(x => x.CityId == _roomForRentAnnouncement.CityId &&
                            x.Created >= _roomForRentAnnouncement.Created &&
                            x.Created <= _roomForRentAnnouncement.Created &&
                            x.Price >= _roomForRentAnnouncement.Price && x.Price <= _roomForRentAnnouncement.Price &&
                            x.CityDistricts.Contains(cityDistrict) &&
                            x.RoomTypes.Select(r => r.ToString()).Contains(roomType.DisplayName))
                .WithPagination(page, pageSize)
                .ToListAsync();
            var roomForRentAnnouncements = roomForRentAnnouncementEntities
                .Select(x => RoomForRentAnnouncement.Builder()
                    .SetId(x.Id)
                    .SetTitle(x.Title)
                    .SetSourceUrl(x.SourceUrl)
                    .SetCityId(x.CityId)
                    .SetCreated(x.Created)
                    .SetDescription(x.Description)
                    .SetPrice(x.Price)
                    .SetCityDistricts(x.CityDistricts)
                    .SetRoomTypes(x.RoomTypes.Select(r => r.ConvertToEnumeration()))
                    .Build()
                )
                .ToList();

            _orderByExpressionCreatorMock
                .Setup(x => x.CreateExpression(It.IsAny<string>()))
                .Returns(OrderByExpression);
            _mapperMock
                .Setup(x => x.Map<List<RoomForRentAnnouncementEntity>, List<RoomForRentAnnouncement>>(It.IsAny<List<RoomForRentAnnouncementEntity>>()))
                .Returns(roomForRentAnnouncements);
            var result = await _repository.FindAsync(page, pageSize, sort, _roomForRentAnnouncement.CityId,
                _roomForRentAnnouncement.Created, _roomForRentAnnouncement.Created, _roomForRentAnnouncement.Price,
                _roomForRentAnnouncement.Price, _roomForRentAnnouncement.CityDistricts.FirstOrDefault(),
                _roomForRentAnnouncement.RoomTypes.FirstOrDefault());

            result.Should().BeEquivalentTo(roomForRentAnnouncements);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_RoomForRentAnnouncement()
        {
            _mapperMock
                .Setup(x => x.Map<RoomForRentAnnouncementEntity, RoomForRentAnnouncement>(It.IsAny<RoomForRentAnnouncementEntity>()))
                .Returns(_roomForRentAnnouncement);

            var result = await _repository.GetByIdAsync(_roomForRentAnnouncement.Id);

            result.Should().BeEquivalentTo(_roomForRentAnnouncement);
        }

        [Fact]
        public async Task AddAsync_Should_Add_RoomForRentAnnouncement()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://sourceUrl")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .Build();
            var roomForRentAnnouncementEntity = new RoomForRentAnnouncementEntity
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
                CosmosEntityName = nameof(RoomForRentAnnouncement)
            };

            _mapperMock
                .Setup(x => x.Map<RoomForRentAnnouncement, RoomForRentAnnouncementEntity>(It.IsAny<RoomForRentAnnouncement>()))
                .Returns(roomForRentAnnouncementEntity);

            await _repository.AddAsync(roomForRentAnnouncement);

            var addedRoomForRentAnnouncementEntity = await _cosmosStore.FindAsync(roomForRentAnnouncement.Id.ToString());
            addedRoomForRentAnnouncementEntity.Should().BeEquivalentTo(roomForRentAnnouncementEntity);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_RoomForRentAnnouncement()
        {
            var roomForRentAnnouncement = await _fixture.InsertRoomForRentAnnouncementAsync();
            roomForRentAnnouncement.ChangeTitle("NewTitle");
            var roomForRentAnnouncementEntity = new RoomForRentAnnouncementEntity
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
                CosmosEntityName = nameof(RoomForRentAnnouncement)
            };

            _mapperMock
                .Setup(x => x.Map<RoomForRentAnnouncement, RoomForRentAnnouncementEntity>(It.IsAny<RoomForRentAnnouncement>()))
                .Returns(roomForRentAnnouncementEntity);

            await _repository.UpdateAsync(roomForRentAnnouncement);

            var updatedRoomForRentAnnouncementEntity = await _cosmosStore.FindAsync(roomForRentAnnouncement.Id.ToString());
            updatedRoomForRentAnnouncementEntity.Should().BeEquivalentTo(roomForRentAnnouncementEntity);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_RoomForRentAnnouncement()
        {
            var roomForRentAnnouncement = await _fixture.InsertRoomForRentAnnouncementAsync();

            await _repository.DeleteAsync(roomForRentAnnouncement);

            var deletedRoomForRentAnnouncementEntity = await _cosmosStore.FindAsync(roomForRentAnnouncement.Id.ToString());
            deletedRoomForRentAnnouncementEntity.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_RoomForRentAnnouncements()
        {
            var expectedResult = await _cosmosStore.Query().CountAsync();

            var result = await _repository.CountAsync();

            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_RoomForRentAnnouncements_For_Given_Parameters()
        {
            var cityDistrict = _roomForRentAnnouncement.CityDistricts.FirstOrDefault();
            var roomType = _roomForRentAnnouncement.RoomTypes.FirstOrDefault();
            var expectedResult = await _cosmosStore.Query().Where(x => x.CityId == _roomForRentAnnouncement.CityId &&
                                                                       x.Created >= _roomForRentAnnouncement.Created &&
                                                                       x.Created <= _roomForRentAnnouncement.Created &&
                                                                       x.Price >= _roomForRentAnnouncement.Price && x.Price <= _roomForRentAnnouncement.Price &&
                                                                       x.CityDistricts.Contains(cityDistrict) &&
                                                                       x.RoomTypes.Select(r => r.ToString()).Contains(roomType.DisplayName)).CountAsync();

            var result = await _repository.CountAsync(_roomForRentAnnouncement.CityId,
                _roomForRentAnnouncement.Created, _roomForRentAnnouncement.Created, _roomForRentAnnouncement.Price,
                _roomForRentAnnouncement.Price, _roomForRentAnnouncement.CityDistricts.FirstOrDefault(),
                _roomForRentAnnouncement.RoomTypes.FirstOrDefault());

            result.Should().Be(expectedResult);
        }
    }
}