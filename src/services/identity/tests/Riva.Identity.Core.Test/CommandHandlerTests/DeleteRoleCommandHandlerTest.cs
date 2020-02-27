using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Commands.Handlers;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.CommandHandlerTests
{
    public class DeleteRoleCommandHandlerTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IRoleGetterService> _roleGetterServiceMock;
        private readonly ICommandHandler<DeleteRoleCommand> _commandHandler;

        public DeleteRoleCommandHandlerTest()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleGetterServiceMock = new Mock<IRoleGetterService>();
            _commandHandler = new DeleteRoleCommandHandler(_roleRepositoryMock.Object, _roleGetterServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_Role()
        {
            var deleteRoleCommand = new DeleteRoleCommand(Guid.NewGuid(), Array.Empty<byte>());
            var role = new Role(deleteRoleCommand.RoleId, deleteRoleCommand.RowVersion, "Name");
            var getRoleResult = GetResult<Role>.Ok(role);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);
            _roleRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteRoleCommand);

            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Role_Is_Not_Found()
        {
            var deleteRoleCommand = new DeleteRoleCommand(Guid.NewGuid(), Array.Empty<byte>());
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.NotFound, RoleErrorMessage.NotFound)
            };
            var getRoleResult = GetResult<Role>.Fail(errors);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteRoleCommand);
            var exceptionResult = await result.Should().ThrowAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_PreconditionFailedException_When_RowVersion_Does_Not_Match()
        {
            var deleteRoleCommand = new DeleteRoleCommand(Guid.NewGuid(), new byte[] { 1, 2, 4, 8, 16, 64 });
            var role = new Role(deleteRoleCommand.RoleId, new byte[] { 1, 2, 4, 8, 16, 32 }, "Name");
            var getRoleResult = GetResult<Role>.Ok(role);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteRoleCommand);

            await result.Should().ThrowAsync<PreconditionFailedException>();
        }
    }
}