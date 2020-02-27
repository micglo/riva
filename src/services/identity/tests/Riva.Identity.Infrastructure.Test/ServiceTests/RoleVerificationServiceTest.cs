using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class RoleVerificationServiceTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly IRoleVerificationService _roleVerificationService;

        public RoleVerificationServiceTest()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleVerificationService = new RoleVerificationService(_roleRepositoryMock.Object);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_True()
        {
            const string name = "RoleName";
            var expectedResult = VerificationResult.Ok();

            _roleRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<Role>(null));

            var result = await _roleVerificationService.VerifyNameIsNotTakenAsync(name);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_False_And_Errors_When_Name_Is_Already_Taken()
        {
            const string name = "RoleName";
            var role = new Role(Guid.NewGuid(), Array.Empty<byte>(), name);
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.NameIsAlreadyTaken, RoleErrorMessage.NameIsAlreadyTaken)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _roleRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            var result = await _roleVerificationService.VerifyNameIsNotTakenAsync(name);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}