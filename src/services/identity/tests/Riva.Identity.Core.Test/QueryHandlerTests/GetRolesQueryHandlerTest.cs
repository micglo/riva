using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Core.Queries;
using Riva.Identity.Core.Queries.Handlers;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.QueryHandlerTests
{
    public class GetRolesQueryHandlerTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetRolesInputQuery, CollectionOutputQuery<RoleOutputQuery>> _queryHandler;

        public GetRolesQueryHandlerTest()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetRolesQueryHandler(_roleRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_RoleOutputQuery()
        {
            var roles = new List<Role>
            {
                new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.Administrator.DisplayName)
            };
            var roleOutputQueries = roles.Select(x => new RoleOutputQuery(x.Id, x.RowVersion, x.Name));
            var collectionOutputQuery = new CollectionOutputQuery<RoleOutputQuery>(roles.Count, roleOutputQueries);

            _roleRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(roles);
            _mapperMock.Setup(x =>
                    x.Map<List<Role>, CollectionOutputQuery<RoleOutputQuery>>(It.IsAny<List<Role>>()))
                .Returns(collectionOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetRolesInputQuery());

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }
    }
}