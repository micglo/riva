using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Commands.Handlers;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.CommandHandlerTests
{
    public class UpdateAccountRolesCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly ICommandHandler<UpdateAccountRolesCommand> _commandHandler;

        public UpdateAccountRolesCommandHandlerTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _commandHandler = new UpdateAccountRolesCommandHandler(_accountRepositoryMock.Object, _roleRepositoryMock.Object,
                _accountGetterServiceMock.Object, _communicationBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_Account_Roles()
        {
            var adminRole = new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.Administrator.DisplayName);
            var userRole = new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.User.DisplayName);
            var roleToRemove = new Role(Guid.NewGuid(), Array.Empty<byte>(), "RoleToRemove");
            var roleToAddId = Guid.NewGuid();
            var roles = new List<Role>
            {
                userRole,
                adminRole,
                new Role(roleToAddId, Array.Empty<byte>(), "RoleToAdd"),
                new Role(roleToRemove.Id, Array.Empty<byte>(), "RoleToRemove")
            };
            var updateAccountRolesCommand = new UpdateAccountRolesCommand(Guid.NewGuid(), new List<Guid> { userRole.Id, adminRole.Id, roleToAddId });
            var account = Account.Builder()
                .SetId(updateAccountRolesCommand.AccountId)
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { userRole.Id, adminRole.Id, roleToRemove.Id })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var cancellationToken = new CancellationToken();

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _roleRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(userRole);
            _roleRepositoryMock.Setup(x => x.FindByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(roles);
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateAccountRolesCommand, cancellationToken);
            
            await result.Should().NotThrowAsync<Exception>();
            account.Roles.Should().Contain(adminRole.Id);
            account.Roles.Should().Contain(userRole.Id);
            account.Roles.Should().Contain(roleToAddId);
            account.Roles.Should().NotContain(roleToRemove.Id);
            _communicationBusMock.Verify(
                x => x.DispatchDomainEventsAsync(It.Is<Account>(a => a == account),
                    It.Is<CancellationToken>(ct => ct == cancellationToken)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Account_Is_Not_Found()
        {
            var userRole = new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.User.DisplayName);
            var roleIdToUpdate = Guid.NewGuid();
            var updateAccountRolesCommand = new UpdateAccountRolesCommand(Guid.NewGuid(), new List<Guid> { userRole.Id, roleIdToUpdate});
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateAccountRolesCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Command_Does_Not_Contain_User_Role()
        {
            var userRole = new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.User.DisplayName);
            var roleIdToUpdate = Guid.NewGuid();
            var updateAccountRolesCommand = new UpdateAccountRolesCommand(Guid.NewGuid(), new List<Guid> { roleIdToUpdate });
            var account = Account.Builder()
                .SetId(updateAccountRolesCommand.AccountId)
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { userRole.Id })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var roles = new List<Role>
            {
                new Role(roleIdToUpdate, Array.Empty<byte>(), "RoleToUpdate")
            };
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.UserRoleIsNotRemovable, AccountErrorMessage.UserRoleIsNotRemovable)
            };

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _roleRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(userRole);
            _roleRepositoryMock.Setup(x => x.FindByIdsAsync(It.IsAny<List<Guid>>())).ReturnsAsync(roles);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateAccountRolesCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Any_Of_Given_Roles_To_Update_Does_Not_Exist()
        {
            var userRole = new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.User.DisplayName);
            var roleIdToUpdate = Guid.NewGuid();
            var updateAccountRolesCommand = new UpdateAccountRolesCommand(Guid.NewGuid(), new List<Guid> { userRole.Id, roleIdToUpdate });
            var account = Account.Builder()
                .SetId(updateAccountRolesCommand.AccountId)
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { userRole.Id })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var roles = new List<Role>
            {
                userRole
            };
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.RolesNotFound, RoleErrorMessage.RolesNotFound)
            };

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _roleRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(userRole);
            _roleRepositoryMock.Setup(x => x.FindByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(roles);

            Func<Task> result = async () => await _commandHandler.HandleAsync(updateAccountRolesCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}