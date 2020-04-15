using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;

namespace Riva.Users.Infrastructure.Services
{
    public class UserVerificationService : IUserVerificationService
    {
        private readonly IUserGetterService _userGetterService;
        private readonly IAuthorizationService _authorizationService;

        public UserVerificationService(IUserGetterService userGetterService, IAuthorizationService authorizationService)
        {
            _userGetterService = userGetterService;
            _authorizationService = authorizationService;
        }

        public async Task<VerificationResult> VerifyUserDoesNotExistAsync(Guid userId)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(userId);
            if (getUserResult.Success)
            {
                var errors = new List<IError>
                {
                    new Error(UserErrorCodeEnumeration.AlreadyExist, UserErrorMessage.AlreadyExist)
                };
                return VerificationResult.Fail(errors);
            }
            return VerificationResult.Ok();
        }

        public VerificationResult VerifyAnnouncementPreferenceLimitCanBeChanged()
        {
            if (_authorizationService.IsAdministrator())
                return VerificationResult.Ok();

            var errors = new Collection<IError>
            {
                new Error(UserErrorCodeEnumeration.InsufficientPrivileges,
                    UserErrorMessage.InsufficientPrivilegesToEditAnnouncementPreferenceLimit)
            };
            return VerificationResult.Fail(errors);
        }

        public VerificationResult VerifyAnnouncementSendingFrequencyCanBeChanged()
        {
            if (_authorizationService.IsAdministrator())
                return VerificationResult.Ok();

            var errors = new Collection<IError>
            {
                new Error(UserErrorCodeEnumeration.InsufficientPrivileges,
                    UserErrorMessage.InsufficientPrivilegesToEditAnnouncementSendingFrequency)
            };
            return VerificationResult.Fail(errors);
        }

        public VerificationResult VerifyAnnouncementPreferenceLimitIsNotExceeded(int announcementPreferenceLimit, int currentAnnouncementPreferencesCount)
        {
            if (currentAnnouncementPreferencesCount == announcementPreferenceLimit)
            {
                var errors = new Collection<IError>
                {
                    new Error(UserErrorCodeEnumeration.AnnouncementPreferenceLimitExceeded,
                        UserErrorMessage.AnnouncementPreferenceLimitExceeded)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }
    }
}