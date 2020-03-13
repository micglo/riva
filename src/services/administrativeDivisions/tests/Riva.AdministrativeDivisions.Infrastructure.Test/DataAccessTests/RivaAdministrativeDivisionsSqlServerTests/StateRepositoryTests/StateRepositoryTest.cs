using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.Models;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests.StateRepositoryTests
{
    [Collection("RivaAdministrativeDivisionsSqlServer tests collection")]
    public class StateRepositoryTest : IClassFixture<StateRepositoryTestFixture>
    {
        private readonly RivaAdministrativeDivisionsDbContext _dbContext;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderByExpressionCreator<State>> _orderByExpressionCreatorMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly IStateRepository _repository;
        private readonly State _state;
        private readonly StateRepositoryTestFixture _fixture;

        public StateRepositoryTest(StateRepositoryTestFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _mapperMock = fixture.MapperMock;
            _orderByExpressionCreatorMock = fixture.OrderByExpressionCreatorMock;
            _memoryCacheMock = fixture.MemoryCacheMock;
            _repository = fixture.Repository;
            _state = fixture.State;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_States_Collection_From_Cache()
        {
            var states = new List<State> {_state };
            object cachedStates = states;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStates)).Returns(true);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(states);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_States_Collection_From_Database()
        {
            var stateEntities = await _dbContext.States.ToListAsync();
            var states = stateEntities.Select(x => State.Builder()
                .SetId(x.Id)
                .SetRowVersion(x.RowVersion)
                .SetName(x.Name)
                .SetPolishName(x.PolishName)
                .Build()).ToList();
            object cachedStates = null;
            var cacheEntryMock  = new Mock<ICacheEntry>();

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStates)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
            _mapperMock.Setup(x => x.Map<List<StateEntity>, List<State>>(It.IsAny<List<StateEntity>>()))
                .Returns(states);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(states);
        }

        [Fact]
        public async Task FindAsync_Should_Return_States_Collection()
        {
            var states = new List<State> { _state };
            object cachedStates = states;
            IOrderedQueryable<State> OrderByExpression(IQueryable<State> o) => o.OrderByDescending(x => x.Name);

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStates)).Returns(true);
            _orderByExpressionCreatorMock.Setup(x => x.CreateExpression(It.IsAny<string>())).Returns(OrderByExpression);

            var result = await _repository.FindAsync(1, 100, "name:asc", _state.Name, _state.PolishName);

            result.Should().BeEquivalentTo(states);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_State()
        {
            _mapperMock.Setup(x => x.Map<StateEntity, State>(It.IsAny<StateEntity>()))
                .Returns(_state);

            var result = await _repository.GetByIdAsync(_state.Id);

            result.Should().BeEquivalentTo(_state);
        }

        [Fact]
        public async Task GetByNameAsync_Should_Return_State()
        {
            _mapperMock.Setup(x => x.Map<StateEntity, State>(It.IsAny<StateEntity>()))
                .Returns(_state);

            var result = await _repository.GetByNameAsync(_state.Name);

            result.Should().BeEquivalentTo(_state);
        }

        [Fact]
        public async Task GetByPolishNameAsync_Should_Return_State()
        {
            _mapperMock.Setup(x => x.Map<StateEntity, State>(It.IsAny<StateEntity>()))
                .Returns(_state);

            var result = await _repository.GetByPolishNameAsync(_state.PolishName);

            result.Should().BeEquivalentTo(_state);
        }

        [Fact]
        public async Task AddAsync_Should_Add_State()
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("StateRepositoryTestAddAsync")
                .SetPolishName("StateRepositoryTestAddAsync")
                .Build();
            var stateEntity = new StateEntity
            {
                Id = state.Id,
                Name = state.Name,
                PolishName = state.PolishName,
                RowVersion = state.RowVersion.ToArray()
            };

            _mapperMock.Setup(x => x.Map<State, StateEntity>(It.IsAny<State>()))
                .Returns(stateEntity);
            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.AddAsync(state);
            await result.Should().NotThrowAsync<Exception>();

            var addedState = await _dbContext.States.FindAsync(state.Id);
            addedState.Should().NotBeNull();
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.StatesKey))));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_State()
        {
            var state = _fixture.InsertState("StateRepositoryTestUpdateAsync");
            state.ChangeName("StateRepositoryTestUpdateAsyncNewName");
            var stateEntity = new StateEntity
            {
                Id = state.Id,
                Name = state.Name,
                PolishName = state.PolishName,
                RowVersion = state.RowVersion.ToArray()
            };

            _mapperMock.Setup(x => x.Map<State, StateEntity>(It.IsAny<State>()))
                .Returns(stateEntity);
            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.UpdateAsync(state);
            await result.Should().NotThrowAsync<Exception>();

            var updatedState = await _dbContext.States.Include(x => x.Cities).SingleOrDefaultAsync(x => x.Id == state.Id);
            updatedState.Name.Should().BeEquivalentTo(state.Name);
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.StatesKey))));
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_State()
        {
            var state = _fixture.InsertState("StateRepositoryTestDeleteAsync");

            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>())).Verifiable();

            Func<Task> result = async () => await _repository.DeleteAsync(state);
            await result.Should().NotThrowAsync<Exception>();

            var deletedState = await _dbContext.States.FindAsync(state.Id);
            deletedState.Should().BeNull();
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.StatesKey))));
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_States()
        {
            var states = new List<State> { _state };
            object cachedStates = states;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStates)).Returns(true);

            var result = await _repository.CountAsync();

            result.Should().Be(states.Count);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_States_For_Given_Name_And_PolishName()
        {
            var states = new List<State> { _state };
            object cachedStates = states;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStates)).Returns(true);

            var result = await _repository.CountAsync(_state.Name, _state.PolishName);

            result.Should().Be(states.Count);
        }
    }
}