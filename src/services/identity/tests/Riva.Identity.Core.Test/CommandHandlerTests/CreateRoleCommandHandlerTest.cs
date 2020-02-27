using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;
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
    public class CreateRoleCommandHandlerTest
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IRoleVerificationService> _roleVerificationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICommandHandler<CreateRoleCommand> _commandHandler;

        public CreateRoleCommandHandlerTest()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleVerificationServiceMock = new Mock<IRoleVerificationService>();
            _mapperMock = new Mock<IMapper>();
            _commandHandler = new CreateRoleCommandHandler(_roleRepositoryMock.Object, _roleVerificationServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_Role()
        {
            var createRoleCommand = new CreateRoleCommand(Guid.NewGuid(), DefaultRoleEnumeration.Administrator.DisplayName);
            var role = new Role(createRoleCommand.RoleId, Array.Empty<byte>(), createRoleCommand.Name);
            var nameIsNotTakenVerificationResult = VerificationResult.Ok();

            _roleVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);
            _mapperMock.Setup(x => x.Map<CreateRoleCommand, Role>(It.IsAny<CreateRoleCommand>())).Returns(role);
            _roleRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Role>())).Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _commandHandler.HandleAsync(createRoleCommand);

            await result.Should().NotThrowAsync<Exception>();
            _roleRepositoryMock.Verify(x => x.AddAsync(It.Is<Role>(r => r == role)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Name_Is_Already_Used()
        {
            var createRoleCommand = new CreateRoleCommand(Guid.NewGuid(), DefaultRoleEnumeration.Administrator.DisplayName);
            var errors = new Collection<IError>
            {
                new Error(RoleErrorCodeEnumeration.NameIsAlreadyTaken, RoleErrorMessage.NameIsAlreadyTaken)
            };
            var nameIsNotTakenVerificationResult = VerificationResult.Fail(errors);

            _roleVerificationServiceMock.Setup(x => x.VerifyNameIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(nameIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createRoleCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}