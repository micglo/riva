using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Queries;
using Riva.Identity.Core.Queries.Handlers;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;
using Xunit;

namespace Riva.Identity.Core.Test.QueryHandlerTests
{
    public class GetRoleQueryHandlerTest
    {
        private readonly Mock<IRoleGetterService> _roleGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetRoleInputQuery, RoleOutputQuery> _queryHandler;

        public GetRoleQueryHandlerTest()
        {
            _roleGetterServiceMock = new Mock<IRoleGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetRoleQueryHandler(_roleGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_RoleOutputQuery()
        {
            var role = new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.Administrator.DisplayName);
            var getRoleResult = GetResult<Role>.Ok(role);
            var roleOutputQuery = new RoleOutputQuery(role.Id, role.RowVersion, role.Name);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);
            _mapperMock.Setup(x => x.Map<Role, RoleOutputQuery>(It.IsAny<Role>())).Returns(roleOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetRoleInputQuery(role.Id));

            result.Should().BeEquivalentTo(roleOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Role_Is_Not_Found()
        {
            var roleId = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.NotFound, RoleErrorMessage.NotFound)
            };
            var getRoleResult = GetResult<Role>.Fail(errors);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);

            Func<Task<RoleOutputQuery>> result = async () => await _queryHandler.HandleAsync(new GetRoleInputQuery(roleId));
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}