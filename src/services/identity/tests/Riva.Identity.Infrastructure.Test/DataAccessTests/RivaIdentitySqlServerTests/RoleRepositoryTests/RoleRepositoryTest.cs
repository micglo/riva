using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.RivaIdentitySqlServerTests.RoleRepositoryTests
{
    [Collection("RivaIdentitySqlServer tests collection")]
    public class RoleRepositoryTest : IClassFixture<RoleRepositoryTestFixture>
    {
        private readonly RivaIdentityDbContext _context;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IRoleRepository _repository;
        private readonly Role _role;
        private readonly RoleRepositoryTestFixture _fixture;

        public RoleRepositoryTest(RoleRepositoryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapperMock = fixture.MapperMock;
            _repository = fixture.Repository;
            _role = fixture.Role;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Roles_Collection()
        {
            var roleEntities = await _context.Roles.ToListAsync();
            var roles = roleEntities.Select(x => new Role(x.Id, x.RowVersion, x.Name)).ToList();

            _mapperMock.Setup(x => x.Map<List<RoleEntity>, List<Role>>(It.IsAny<List<RoleEntity>>()))
                .Returns(roles);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public async Task FindByIdsAsync_Should_Return_Roles_Collection()
        {
            var roleEntities = await _context.Roles.ToListAsync();
            var roles = roleEntities.Select(x => new Role(x.Id, x.RowVersion, x.Name)).ToList();

            _mapperMock.Setup(x => x.Map<List<RoleEntity>, List<Role>>(It.IsAny<List<RoleEntity>>()))
                .Returns(roles);

            var result = await _repository.FindByIdsAsync(roles.Select(x => x.Id));

            result.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Role()
        {
            _mapperMock.Setup(x => x.Map<RoleEntity, Role>(It.IsAny<RoleEntity>()))
                .Returns(_role);

            var result = await _repository.GetByIdAsync(_role.Id);

            result.Should().BeEquivalentTo(_role);
        }

        [Fact]
        public async Task GetByNameAsync_Should_Return_Role()
        {
            _mapperMock.Setup(x => x.Map<RoleEntity, Role>(It.IsAny<RoleEntity>()))
                .Returns(_role);

            var result = await _repository.GetByNameAsync(_role.Name);

            result.Should().BeEquivalentTo(_role);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Role()
        {
            var role = new Role(Guid.NewGuid(), Array.Empty<byte>(), "RoleRepositoryTestAddAsync");
            var roleEntity = new RoleEntity
            {
                Id = role.Id,
                Name = role.Name,
                RowVersion = role.RowVersion.ToArray()
            };

            _mapperMock.Setup(x => x.Map<Role, RoleEntity>(It.IsAny<Role>()))
                .Returns(roleEntity);

            Func<Task> result = async () => await _repository.AddAsync(role);
            await result.Should().NotThrowAsync<Exception>();

            var addedRole = await _context.Roles.FindAsync(role.Id);
            addedRole.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Role()
        {
            const string newName = "RoleRepositoryTestUpdateAsyncNewName";
            var role = _fixture.InsertRole("RoleRepositoryTestUpdateAsync");
            role.ChangeName(newName);
            var roleEntity = new RoleEntity
            {
                Id = role.Id,
                Name = role.Name,
                RowVersion = role.RowVersion.ToArray()
            };

            _mapperMock.Setup(x => x.Map<Role, RoleEntity>(It.IsAny<Role>()))
                .Returns(roleEntity);

            Func<Task> result = async () => await _repository.UpdateAsync(role);
            await result.Should().NotThrowAsync<Exception>();

            var updatedRole = await _context.Roles.FindAsync(role.Id);
            updatedRole.Name.Should().BeEquivalentTo(newName);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Role()
        {
            var role = _fixture.InsertRole("RoleRepositoryTestDeleteAsync");
            var roleEntity = new RoleEntity
            {
                Id = role.Id,
                Name = role.Name,
                RowVersion = role.RowVersion.ToArray()
            };

            _mapperMock.Setup(x => x.Map<Role, RoleEntity>(It.IsAny<Role>()))
                .Returns(roleEntity);

            Func<Task> result = async () => await _repository.DeleteAsync(role);
            await result.Should().NotThrowAsync<Exception>();

            var deletedRole = await _context.Roles.FindAsync(role.Id);
            deletedRole.Should().BeNull();
        }
    }
}