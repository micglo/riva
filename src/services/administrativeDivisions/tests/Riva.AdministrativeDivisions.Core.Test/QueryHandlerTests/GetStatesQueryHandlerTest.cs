using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Core.Queries.Handlers;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.QueryHandlerTests
{
    public class GetStatesQueryHandlerTest
    {
        private readonly Mock<IStateRepository> _stateRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetStatesInputQuery, CollectionOutputQuery<StateOutputQuery>> _queryHandler;

        public GetStatesQueryHandlerTest()
        {
            _stateRepositoryMock = new Mock<IStateRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetStatesQueryHandler(_stateRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_StateOutputQuery_When_Input_Is_Null()
        {
            var states = new List<State>
            {
                State.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .Build()
            };
            var stateOutputQueries = states.Select(x => new StateOutputQuery(x.Id, x.RowVersion, x.Name, x.PolishName)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<StateOutputQuery>(states.Count, stateOutputQueries);

            _stateRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(states);
            _stateRepositoryMock.Setup(x => x.CountAsync()).ReturnsAsync(states.Count);
            _mapperMock.Setup(x => x.Map<List<State>, IEnumerable<StateOutputQuery>>(It.IsAny<List<State>>()))
                .Returns(stateOutputQueries);

            var result = await _queryHandler.HandleAsync(null);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_StateOutputQuery_When_Input_Is_Not_Null()
        {
            var getStatesInputQuery = new GetStatesInputQuery(1, 100, "name:asc", "Name", "PolishName");
            var states = new List<State>
            {
                State.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .Build()
            };
            var stateOutputQueries = states
                .Select(x => new StateOutputQuery(x.Id, x.RowVersion, x.Name, x.PolishName)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<StateOutputQuery>(states.Count, stateOutputQueries);

            _stateRepositoryMock.Setup(x => x.FindAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(states);
            _stateRepositoryMock.Setup(x => x.CountAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(states.Count);
            _mapperMock.Setup(x => x.Map<List<State>, IEnumerable<StateOutputQuery>>(It.IsAny<List<State>>()))
                .Returns(stateOutputQueries);

            var result = await _queryHandler.HandleAsync(getStatesInputQuery);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }
    }
}