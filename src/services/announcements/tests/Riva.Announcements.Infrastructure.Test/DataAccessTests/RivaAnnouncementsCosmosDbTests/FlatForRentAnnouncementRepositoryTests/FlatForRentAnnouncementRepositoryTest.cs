using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using FluentAssertions;
using Moq;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Xunit;

namespace Riva.Announcements.Infrastructure.Test.DataAccessTests.RivaAnnouncementsCosmosDbTests.FlatForRentAnnouncementRepositoryTests                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
{
    [Collection("RivaAnnouncementsCosmosDb tests collection")]
    public class FlatForRentAnnouncementRepositoryTest : IClassFixture<FlatForRentAnnouncementRepositoryTestFixture>
    {
        private readonly IFlatForRentAnnouncementRepository _repository;
        private readonly ICosmosStore<FlatForRentAnnouncementEntity> _cosmosStore;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderByExpressionCreator<FlatForRentAnnouncementEntity>> _orderByExpressionCreatorMock;
        private readonly FlatForRentAnnouncement _flatForRentAnnouncement;
        private readonly FlatForRentAnnouncementRepositoryTestFixture _fixture;

        public FlatForRentAnnouncementRepositoryTest(FlatForRentAnnouncementRepositoryTestFixture fixture)
        {
            _cosmosStore = fixture.CosmosStore;
            _mapperMock = fixture.MapperMock;
            _orderByExpressionCreatorMock = fixture.OrderByExpressionCreatorMock;
            _repository = fixture.Repository;
            _flatForRentAnnouncement = fixture.FlatForRentAnnouncement;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_FlatForRentAnnouncements()
        {
            var flatForRentAnnouncementEntities = await _cosmosStore.Query().ToListAsync();
            var flatForRentAnnouncements = flatForRentAnnouncementEntities.Select(x => FlatForRentAnnouncement.Builder()
                .SetId(x.Id)
                .SetTitle(x.Title)
                .SetSourceUrl(x.SourceUrl)
                .SetCityId(x.CityId)
                .SetCreated(x.Created)
                .SetDescription(x.Description)
                .SetNumberOfRooms(x.NumberOfRooms.ConvertToEnumeration())
                .SetPrice(x.Price)
                .SetCityDistricts(x.CityDistricts)
                .Build())
                .ToList();

            _mapperMock
                .Setup(x => x.Map<List<FlatForRentAnnouncementEntity>, List<FlatForRentAnnouncement>>(It.IsAny<List<FlatForRentAnnouncementEntity>>()))
                .Returns(flatForRentAnnouncements);
            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(flatForRentAnnouncements);
        }

        [Fact]
        public async Task FindAsync_Should_Return_FlatForRentAnnouncements()
        {
            const int page = 1;
            const int pageSize = 3;
            const string sort = "created:desc";
            static IOrderedQueryable<FlatForRentAnnouncementEntity> OrderByExpression(
                IQueryable<FlatForRentAnnouncementEntity> o) => o.OrderByDescending(x => x.Created);
            var cityDistrict = _flatForRentAnnouncement.CityDistricts.FirstOrDefault();

            var flatForRentAnnouncementEntities = await _cosmosStore.Query()
                .OrderByDescending(x => x.Created)
                .Where(x =>
                    x.CityId == _flatForRentAnnouncement.CityId && x.Created >= _flatForRentAnnouncement.Created &&
                    x.Created <= _flatForRentAnnouncement.Created && x.Price >= _flatForRentAnnouncement.Price &&
                    x.Price <= _flatForRentAnnouncement.Price &&
                    x.NumberOfRooms.ToString().Equals(_flatForRentAnnouncement.NumberOfRooms.DisplayName) &&
                    x.CityDistricts.Contains(cityDistrict))
                .WithPagination(page, pageSize)
                .ToListAsync();
            var flatForRentAnnouncements = flatForRentAnnouncementEntities
                .Select(x => FlatForRentAnnouncement.Builder()
                    .SetId(x.Id)
                    .SetTitle(x.Title)
                    .SetSourceUrl(x.SourceUrl)
                    .SetCityId(x.CityId)
                    .SetCreated(x.Created)
                    .SetDescription(x.Description)
                    .SetNumberOfRooms(x.NumberOfRooms.ConvertToEnumeration())
                    .SetPrice(x.Price)
                    .SetCityDistricts(x.CityDistricts)
                    .Build()
                )
                .ToList();

            _orderByExpressionCreatorMock
                .Setup(x => x.CreateExpression(It.IsAny<string>()))
                .Returns(OrderByExpression);
            _mapperMock
                .Setup(x => x.Map<List<FlatForRentAnnouncementEntity>, List<FlatForRentAnnouncement>>(It.IsAny<List<FlatForRentAnnouncementEntity>>()))
                .Returns(flatForRentAnnouncements);
            var result = await _repository.FindAsync(page, pageSize, sort, _flatForRentAnnouncement.CityId,
                _flatForRentAnnouncement.Created, _flatForRentAnnouncement.Created, _flatForRentAnnouncement.Price,
                _flatForRentAnnouncement.Price, _flatForRentAnnouncement.CityDistricts.FirstOrDefault(),
                _flatForRentAnnouncement.NumberOfRooms);

            result.Should().BeEquivalentTo(flatForRentAnnouncements);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_FlatForRentAnnouncement()
        {
            _mapperMock
                .Setup(x => x.Map<FlatForRentAnnouncementEntity, FlatForRentAnnouncement>(It.IsAny<FlatForRentAnnouncementEntity>()))
                .Returns(_flatForRentAnnouncement);

            var result = await _repository.GetByIdAsync(_flatForRentAnnouncement.Id);

            result.Should().BeEquivalentTo(_flatForRentAnnouncement);
        }

        [Fact]
        public async Task AddAsync_Should_Add_FlatForRentAnnouncement()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://sourceUrl")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .Build();
            var flatForRentAnnouncementEntity = new FlatForRentAnnouncementEntity
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
                CosmosEntityName = nameof(FlatForRentAnnouncement)
            };

            _mapperMock
                .Setup(x => x.Map<FlatForRentAnnouncement, FlatForRentAnnouncementEntity>(It.IsAny<FlatForRentAnnouncement>()))
                .Returns(flatForRentAnnouncementEntity);

            await _repository.AddAsync(flatForRentAnnouncement);

            var addedFlatForRentAnnouncementEntity = await _cosmosStore.FindAsync(flatForRentAnnouncement.Id.ToString());
            addedFlatForRentAnnouncementEntity.Should().BeEquivalentTo(flatForRentAnnouncementEntity);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_FlatForRentAnnouncement()
        {
            var flatForRentAnnouncement = await _fixture.InsertFlatForRentAnnouncementAsync();
            flatForRentAnnouncement.ChangeTitle("NewTitle");
            var flatForRentAnnouncementEntity = new FlatForRentAnnouncementEntity
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
                CosmosEntityName = nameof(FlatForRentAnnouncement)
            };

            _mapperMock
                .Setup(x => x.Map<FlatForRentAnnouncement, FlatForRentAnnouncementEntity>(It.IsAny<FlatForRentAnnouncement>()))
                .Returns(flatForRentAnnouncementEntity);

            await _repository.UpdateAsync(flatForRentAnnouncement);

            var updatedFlatForRentAnnouncementEntity = await _cosmosStore.FindAsync(flatForRentAnnouncement.Id.ToString());
            updatedFlatForRentAnnouncementEntity.Should().BeEquivalentTo(flatForRentAnnouncementEntity);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_FlatForRentAnnouncement()
        {
            var flatForRentAnnouncement = await _fixture.InsertFlatForRentAnnouncementAsync();

            await _repository.DeleteAsync(flatForRentAnnouncement);

            var deletedFlatForRentAnnouncementEntity = await _cosmosStore.FindAsync(flatForRentAnnouncement.Id.ToString());
            deletedFlatForRentAnnouncementEntity.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_FlatForRentAnnouncements()
        {
            var expectedResult = await _cosmosStore.Query().CountAsync();

            var result = await _repository.CountAsync();

            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_FlatForRentAnnouncements_For_Given_Parameters()
        {
            var cityDistrict = _flatForRentAnnouncement.CityDistricts.FirstOrDefault();
            var expectedResult = await _cosmosStore.Query().Where(x =>
                x.CityId == _flatForRentAnnouncement.CityId && x.Created >= _flatForRentAnnouncement.Created &&
                x.Created <= _flatForRentAnnouncement.Created && x.Price >= _flatForRentAnnouncement.Price &&
                x.Price <= _flatForRentAnnouncement.Price &&
                x.NumberOfRooms.ToString().Equals(_flatForRentAnnouncement.NumberOfRooms.DisplayName) &&
                x.CityDistricts.Contains(cityDistrict)).CountAsync();

            var result = await _repository.CountAsync(_flatForRentAnnouncement.CityId, _flatForRentAnnouncement.Created,
                _flatForRentAnnouncement.Created, _flatForRentAnnouncement.Price, _flatForRentAnnouncement.Price,
                _flatForRentAnnouncement.CityDistricts.FirstOrDefault(), _flatForRentAnnouncement.NumberOfRooms);

            result.Should().Be(expectedResult);
        }
    }
}