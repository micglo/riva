using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests.StateRepositoryTests
{
    public class StateRepositoryTestFixture
    {
        public RivaAdministrativeDivisionsDbContext DbContext { get; }
        public Mock<IMapper> MapperMock { get; }
        public Mock<IOrderByExpressionCreator<State>> OrderByExpressionCreatorMock { get; }
        public Mock<IMemoryCache> MemoryCacheMock { get; }
        public IStateRepository Repository { get; }
        public State State { get; }

        public StateRepositoryTestFixture(DatabaseFixture fixture)
        {
            DbContext = fixture.DbContext;
            MapperMock = new Mock<IMapper>();
            OrderByExpressionCreatorMock = new Mock<IOrderByExpressionCreator<State>>();
            MemoryCacheMock = new Mock<IMemoryCache>();
            Repository = new StateRepository(DbContext, MapperMock.Object, OrderByExpressionCreatorMock.Object, MemoryCacheMock.Object);
            State = InsertState("StateRepositoryTest");
        }

        public State InsertState(string name)
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(new byte[] { 1, 2, 4, 8, 16, 32, 64 })
                .SetName(name)
                .SetPolishName(name)
                .Build();
            var stateEntity = new StateEntity
            {
                Id = state.Id,
                Name = state.Name,
                PolishName = state.PolishName,
                RowVersion = state.RowVersion.ToArray()
            };
            DbContext.States.Add(stateEntity);
            DbContext.SaveChanges();
            return state;
        }
    }
}