using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class UserVerificationServiceTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly IUserVerificationService _service;

        public UserVerificationServiceTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _service = new UserVerificationService(_userGetterServiceMock.Object, _authorizationServiceMock.Object);
        }

        [Fact]
        public async Task VerifyUserDoesNotExistAsync_Should_Return_VerificationResult_With_Success_True()
        {
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
            };
            var getUserResult = GetResult<User>.Fail(errors);
            var expectedResult = VerificationResult.Ok();

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);

            var result = await _service.VerifyUserDoesNotExistAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyUserDoesNotExistAsync_Should_Return_VerificationResult_With_Success_False()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.AlreadyExist, UserErrorMessage.AlreadyExist)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);

            var result = await _service.VerifyUserDoesNotExistAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAnnouncementPreferenceLimitCanBeChanged_Should_Return_VerificationResult_With_Success_True()
        {
            _authorizationServiceMock.Setup(x => x.IsAdministrator()).Returns(true);
            var expectedResult = VerificationResult.Ok();

            var result = _service.VerifyAnnouncementPreferenceLimitCanBeChanged();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAnnouncementPreferenceLimitCanBeChanged_Should_Return_VerificationResult_With_Success_False()
        {
            _authorizationServiceMock.Setup(x => x.IsAdministrator()).Returns(false);
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.InsufficientPrivileges,
                    UserErrorMessage.InsufficientPrivilegesToEditAnnouncementPreferenceLimit)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyAnnouncementPreferenceLimitCanBeChanged();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAnnouncementSendingFrequencyCanBeChanged_Should_Return_VerificationResult_With_Success_True()
        {
            _authorizationServiceMock.Setup(x => x.IsAdministrator()).Returns(true);
            var expectedResult = VerificationResult.Ok();

            var result = _service.VerifyAnnouncementSendingFrequencyCanBeChanged();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAnnouncementSendingFrequencyCanBeChanged_Should_Return_VerificationResult_With_Success_False()
        {
            _authorizationServiceMock.Setup(x => x.IsAdministrator()).Returns(false);
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.InsufficientPrivileges,
                    UserErrorMessage.InsufficientPrivilegesToEditAnnouncementSendingFrequency)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyAnnouncementSendingFrequencyCanBeChanged();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAnnouncementPreferenceLimitIsNotExceeded_Should_Return_VerificationResult_With_Success_True()
        {
            const int announcementPreferenceLimit = 10;
            const int currentAnnouncementPreferencesCount = 1;
            var expectedResult = VerificationResult.Ok();

            var result = _service.VerifyAnnouncementPreferenceLimitIsNotExceeded(announcementPreferenceLimit,
                currentAnnouncementPreferencesCount);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAnnouncementPreferenceLimitIsNotExceeded_Should_Return_VerificationResult_With_Success_False()
        {
            const int announcementPreferenceLimit = 10;
            const int currentAnnouncementPreferencesCount = 10;
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.AnnouncementPreferenceLimitExceeded,
                    UserErrorMessage.AnnouncementPreferenceLimitExceeded)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyAnnouncementPreferenceLimitIsNotExceeded(announcementPreferenceLimit,
                currentAnnouncementPreferencesCount);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}