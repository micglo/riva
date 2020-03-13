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
    public class StateVerificationServiceTest
    {
        private readonly Mock<IStateRepository> _stateRepositoryMock;
        private readonly IStateVerificationService _stateVerificationService;

        public StateVerificationServiceTest()
        {
            _stateRepositoryMock = new Mock<IStateRepository>();
            _stateVerificationService = new StateVerificationService(_stateRepositoryMock.Object);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_True()
        {
            const string name = "Name";
            var expectedResult = VerificationResult.Ok();

            _stateRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<State>(null));

            var result = await _stateVerificationService.VerifyNameIsNotTakenAsync(name);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_False_And_Errors_When_Name_Is_Already_Taken()
        {
            const string name = "Name";
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName(name)
                .SetPolishName("PolishName")
                .Build();
            var expectedResult = VerificationResult.Fail(new Collection<IError>
                { new Error(StateErrorCodeEnumeration.NameAlreadyInUse, StateErrorMessage.NameAlreadyInUse) });

            _stateRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(state);

            var result = await _stateVerificationService.VerifyNameIsNotTakenAsync(name);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyPolishNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_True()
        {
            const string polishName = "PolishName";
            var expectedResult = VerificationResult.Ok();

            _stateRepositoryMock.Setup(x => x.GetByPolishNameAsync(It.IsAny<string>())).Returns(Task.FromResult<State>(null));

            var result = await _stateVerificationService.VerifyPolishNameIsNotTakenAsync(polishName);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyPolishNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_False_And_Errors_When_PolishName_Is_Already_Taken()
        {
            const string polishName = "PolishName";
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName(polishName)
                .Build();
            var expectedResult = VerificationResult.Fail(new Collection<IError>
                { new Error(StateErrorCodeEnumeration.PolishNameAlreadyInUse, StateErrorMessage.PolishNameAlreadyInUse) });

            _stateRepositoryMock.Setup(x => x.GetByPolishNameAsync(It.IsAny<string>())).ReturnsAsync(state);

            var result = await _stateVerificationService.VerifyPolishNameIsNotTakenAsync(polishName);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}