using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Core.Queries.Handlers;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.QueryHandlerTests
{
    public class GetStateQueryHandlerTest
    {
        private readonly Mock<IStateGetterService> _stateGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetStateInputQuery, StateOutputQuery> _queryHandler;

        public GetStateQueryHandlerTest()
        {
            _stateGetterServiceMock = new Mock<IStateGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetStateQueryHandler(_stateGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_StateOutputQuery()
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();
            var getStateResult = GetResult<State>.Ok(state);
            var stateOutputQuery = new StateOutputQuery(state.Id, state.RowVersion, state.Name, state.PolishName);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);
            _mapperMock.Setup(x => x.Map<State, StateOutputQuery>(It.IsAny<State>())).Returns(stateOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetStateInputQuery(state.Id));

            result.Should().BeEquivalentTo(stateOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_State_Does_Not_Exist()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(StateErrorCodeEnumeration.NotFound, StateErrorMessage.NotFound)
            };
            var getStateResult = GetResult<State>.Fail(errors);

            _stateGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getStateResult);

            Func<Task<StateOutputQuery>> result = async () => await _queryHandler.HandleAsync(new GetStateInputQuery(id));
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}