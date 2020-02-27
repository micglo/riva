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
using Riva.BuildingBlocks.Domain;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class RoleGetterServiceTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly IRoleGetterService _roleGetterService;

        public RoleGetterServiceTest()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleGetterService = new RoleGetterService(_roleRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_Role()
        {
            var id = Guid.NewGuid();
            var role = new Role(id, Array.Empty<byte>(), DefaultRoleEnumeration.Administrator.DisplayName);
            var expectedResult = GetResult<Role>.Ok(role);

            _roleRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(role);

            var result = await _roleGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_Errors_When_Role_Is_Not_Found()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.NotFound, RoleErrorMessage.NotFound)
            };
            var expectedResult = GetResult<Role>.Fail(errors);

            _roleRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Role>(null));

            var result = await _roleGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}