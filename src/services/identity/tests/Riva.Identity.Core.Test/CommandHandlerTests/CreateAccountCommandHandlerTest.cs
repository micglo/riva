using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Commands.Handlers;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Xunit;

namespace Riva.Identity.Core.Test.CommandHandlerTests
{
    public class CreateAccountCommandHandlerTest
    {
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<IAccountCreatorService> _accountCreatorServiceMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly ICommandHandler<CreateAccountCommand> _commandHandler;

        public CreateAccountCommandHandlerTest()
        {
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _accountCreatorServiceMock = new Mock<IAccountCreatorService>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _commandHandler = new CreateAccountCommandHandler(_accountVerificationServiceMock.Object,
                _accountCreatorServiceMock.Object, _integrationEventBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_Account()
        {
            var createAccountCommand = new CreateAccountCommand("email@email.com", "Password1234");
            var emailIsNotTakenVerificationResult = VerificationResult.Ok();
            var account = Account.Builder()
                .SetId(createAccountCommand.AccountId)
                .SetEmail(createAccountCommand.Email)
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();

            _accountVerificationServiceMock.Setup(x => x.VerifyEmailIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(emailIsNotTakenVerificationResult);
            _accountCreatorServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(account)
                .Verifiable();
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>())).Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _commandHandler.HandleAsync(createAccountCommand);

            await result.Should().NotThrowAsync<Exception>();
            _accountCreatorServiceMock
                .Verify(x => x.CreateAsync(It.Is<Guid>(g => g == createAccountCommand.AccountId),
                    It.Is<string>(s => s.Equals(createAccountCommand.Email)),
                    It.Is<string>(s => s.Equals(createAccountCommand.Password)),
                    It.Is<Guid>(c => c == createAccountCommand.CorrelationId)));
            _integrationEventBusMock
                .Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie => ie.CorrelationId == createAccountCommand.CorrelationId)));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_Email_Is_Already_Taken()
        {
            var createAccountCommand = new CreateAccountCommand("email@email.com", "Password1234");
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.EmailIsAlreadyTaken, AccountErrorMessage.EmailIsAlreadyTaken)
            };
            var emailIsNotTakenVerificationResult = VerificationResult.Fail(errors);

            _accountVerificationServiceMock.Setup(x => x.VerifyEmailIsNotTakenAsync(It.IsAny<string>()))
                .ReturnsAsync(emailIsNotTakenVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createAccountCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);

        }
    }
}