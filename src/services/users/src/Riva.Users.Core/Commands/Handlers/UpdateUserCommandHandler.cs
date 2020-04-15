using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Users.Core.Extensions;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Commands.Handlers
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly IUserVerificationService _userVerificationService;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly IBlobContainerService _blobContainerService;

        public UpdateUserCommandHandler(IUserGetterService userGetterService, IUserVerificationService userVerificationService,
            ICommunicationBus communicationBus, IUserRepository userRepository, IIntegrationEventBus integrationEventBus, 
            IBlobContainerService blobContainerService)
        {
            _userGetterService = userGetterService;
            _userVerificationService = userVerificationService;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
            _blobContainerService = blobContainerService;
        }

        public async Task HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if(!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            UpdateAnnouncementPreferenceLimit(getUserResult.Value, command.AnnouncementPreferenceLimit, command.CorrelationId);
            UpdateAnnouncementSendingFrequency(getUserResult.Value, command.AnnouncementSendingFrequency, command.CorrelationId);
            getUserResult.Value.ChangeServiceActive(command.ServiceActive, command.CorrelationId);

            if (command.Picture != null && command.Picture.Data.Any())
            {
                var pictureUrl = await _blobContainerService.UploadFileAsync(command.Picture.Data,
                    $"image-{getUserResult.Value.Id.ToString().ToLower()}", command.Picture.ContentType);
                getUserResult.Value.ChangePicture(pictureUrl, command.CorrelationId);
            }

            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.UpdateAsync(getUserResult.Value);
            var userUpdatedIntegrationEvent = new UserUpdatedIntegrationEvent(command.CorrelationId,
                getUserResult.Value.Id, getUserResult.Value.ServiceActive,
                getUserResult.Value.AnnouncementSendingFrequency.ConvertToEnum());
            await _integrationEventBus.PublishIntegrationEventAsync(userUpdatedIntegrationEvent);
        }

        private void UpdateAnnouncementPreferenceLimit(User user, int announcementPreferenceLimit, Guid correlationId)
        {
            if (user.AnnouncementPreferenceLimit != announcementPreferenceLimit)
            {
                var announcementPreferenceLimitCanBeChangedVerificationResult = _userVerificationService.VerifyAnnouncementPreferenceLimitCanBeChanged();
                if (!announcementPreferenceLimitCanBeChangedVerificationResult.Success)
                    throw new ValidationException(announcementPreferenceLimitCanBeChangedVerificationResult.Errors);

                user.ChangeAnnouncementPreferenceLimit(announcementPreferenceLimit, correlationId);
            }
        }

        private void UpdateAnnouncementSendingFrequency(User user, AnnouncementSendingFrequencyEnumeration announcementSendingFrequency, Guid correlationId)
        {
            if (!Equals(user.AnnouncementSendingFrequency, announcementSendingFrequency))
            {
                var announcementSendingFrequencyCanBeChangedVerificationResult = _userVerificationService.VerifyAnnouncementSendingFrequencyCanBeChanged();
                if (!announcementSendingFrequencyCanBeChangedVerificationResult.Success)
                    throw new ValidationException(announcementSendingFrequencyCanBeChangedVerificationResult.Errors);

                user.ChangeAnnouncementSendingFrequency(announcementSendingFrequency, correlationId);
            }
        }
    }
}