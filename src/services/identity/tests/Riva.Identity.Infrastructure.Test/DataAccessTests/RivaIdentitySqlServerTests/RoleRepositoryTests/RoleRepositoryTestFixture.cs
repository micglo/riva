using System;
using System.Linq;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Repositories;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.RivaIdentitySqlServerTests.RoleRepositoryTests
{
    public class RoleRepositoryTestFixture
    {
        public RivaIdentityDbContext Context { get; }
        public Mock<IMapper> MapperMock { get; }
        public IRoleRepository Repository { get; }
        public Role Role { get; }

        public RoleRepositoryTestFixture(DatabaseFixture fixture)
        {
            Context = fixture.Context;
            MapperMock = new Mock<IMapper>();
            Repository = new RoleRepository(Context, MapperMock.Object);
            Role = InsertRole("RoleRepositoryTest");
        }

        public Role InsertRole(string name)
        {
            var role = new Role(Guid.NewGuid(), new byte[] { 1, 2, 4, 8, 16, 64 }, name);
            var roleEntity = new RoleEntity
            {
                Id = role.Id,
                Name = role.Name,
                RowVersion = role.RowVersion.ToArray()
            };
            Context.Roles.Add(roleEntity);
            Context.SaveChanges();
            return role;
        }
    }
}