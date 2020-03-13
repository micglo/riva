using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.ServiceTests
{
    public class StateGetterServiceTest
    {
        private readonly Mock<IStateRepository> _stateRepositoryMock;
        private readonly IStateGetterService _stateGetterService;

        public StateGetterServiceTest()
        {
            _stateRepositoryMock = new Mock<IStateRepository>();
            _stateGetterService = new StateGetterService(_stateRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_State()
        {
            var id = Guid.NewGuid();
            var state = State.Builder()
                .SetId(id)
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            var expectedResult = GetResult<State>.Ok(state);

            _stateRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(state);

            var result = await _stateGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_Errors_When_State_Is_Not_Found()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(StateErrorCodeEnumeration.NotFound, StateErrorMessage.NotFound)
            };
            var expectedResult = GetResult<State>.Fail(errors);

            _stateRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<State>(null));

            var result = await _stateGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}