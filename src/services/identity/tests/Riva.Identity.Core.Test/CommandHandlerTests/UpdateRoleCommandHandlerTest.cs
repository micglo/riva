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
    public class UpdateRoleCommandHandlerTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IRoleGetterService> _roleGetterServiceMock;
        private readonly Mock<IRoleVerificationService> _roleVerificationServiceMock;
        private readonly ICommandHandler<UpdateRoleCommand> _commandHandler;

        public UpdateRoleCommandHandlerTest()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleGetterServiceMock = new Mock<IRoleGetterService>();
            _roleVerificationServiceMock = new Mock<IRoleVerificationService>();
            _commandHandler = new UpdateRoleCommandHandler(_roleRepositoryMock.Object, _roleGetterServiceMock.Object,
                _roleVerificationServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_Role()
        {
            var updateRoleCommand = new UpdateRoleCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName");
            var role = new Role(updateRoleCommand.RoleId, updateRoleCommand.RowVersion, "OldName");
            var getRoleResult = GetResult<Role>.Ok(role);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);
            _roleVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _roleRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoleCommand);

            await result.Should().NotThrowAsync<Exception>();
            role.Name.Should().BeEquivalentTo(updateRoleCommand.Name);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Role_Is_Not_Found()
        {
            var updateRoleCommand = new UpdateRoleCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName");
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.NotFound, RoleErrorMessage.NotFound)
            };
            var getRoleResult = GetResult<Role>.Fail(errors);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoleCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_PreconditionFailedException_When_RowVersion_Does_Not_Match()
        {
            var updateRoleCommand = new UpdateRoleCommand(Guid.NewGuid(), new byte[] { 1, 2, 4, 8, 16, 64 }, "NewName");
            var role = new Role(updateRoleCommand.RoleId, new byte[] { 1, 2, 4, 8, 16, 32 }, "OldName");
            var getRoleResult = GetResult<Role>.Ok(role);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoleCommand);

            await result.Should().ThrowAsync<PreconditionFailedException>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Name_Is_Already_Used()
        {
            var updateRoleCommand = new UpdateRoleCommand(Guid.NewGuid(), Array.Empty<byte>(), "NewName");
            var role = new Role(updateRoleCommand.RoleId, updateRoleCommand.RowVersion, "OldName");
            var getRoleResult = GetResult<Role>.Ok(role);
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.NameIsAlreadyTaken, RoleErrorMessage.NameIsAlreadyTaken)
            };
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(errors);

            _roleGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getRoleResult);
            _roleVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateRoleCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}