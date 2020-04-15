﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Commands.Handlers;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Repositories;
using Xunit;

namespace Riva.Users.Core.Test.CommandHandlerTests
{
    public class CreateRoomForRentAnnouncementPreferenceCommandHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<IUserVerificationService> _userVerificationServiceMock;
        private readonly Mock<IRoomForRentAnnouncementPreferenceVerificationService> _roomForRentAnnouncementPreferenceVerificationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly ICommandHandler<CreateRoomForRentAnnouncementPreferenceCommand> _commandHandler;

        public CreateRoomForRentAnnouncementPreferenceCommandHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _userVerificationServiceMock = new Mock<IUserVerificationService>();
            _roomForRentAnnouncementPreferenceVerificationServiceMock = new Mock<IRoomForRentAnnouncementPreferenceVerificationService>();
            _mapperMock = new Mock<IMapper>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _commandHandler = new CreateRoomForRentAnnouncementPreferenceCommandHandler(_userGetterServiceMock.Object,
                _cityVerificationServiceMock.Object, _userVerificationServiceMock.Object,
                _roomForRentAnnouncementPreferenceVerificationServiceMock.Object, _mapperMock.Object,
                _communicationBusMock.Object, _userRepositoryMock.Object, _integrationEventBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_Room_For_Rent_Announcement_Preference()
        {
            var command = new CreateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(), 1, 1000,
                RoomTypeEnumeration.Single, new List<Guid> {Guid.NewGuid()});
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var announcementPreferenceLimitIsNotExceededVerificationResult = VerificationResult.Ok();
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(command.PriceMin)
                .SetPriceMax(command.PriceMax)
                .SetRoomType(command.RoomType)
                .SetCityDistricts(command.CityDistricts)
                .Build();
            var roomForRentAnnouncementPreferencesVerificationResult = VerificationResult.Ok();


            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
            _userVerificationServiceMock
                .Setup(x => x.VerifyAnnouncementPreferenceLimitIsNotExceeded(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(announcementPreferenceLimitIsNotExceededVerificationResult);
            _mapperMock.Setup(x =>
                    x.Map<CreateRoomForRentAnnouncementPreferenceCommand, RoomForRentAnnouncementPreference>(
                        It.IsAny<CreateRoomForRentAnnouncementPreferenceCommand>()))
                .Returns(roomForRentAnnouncementPreference);
            _roomForRentAnnouncementPreferenceVerificationServiceMock
                .Setup(x => x.VerifyRoomForRentAnnouncementPreferences(
                    It.IsAny<IEnumerable<RoomForRentAnnouncementPreference>>()))
                .Returns(roomForRentAnnouncementPreferencesVerificationResult);
            _communicationBusMock
                .Setup(x => x.DispatchDomainEventsAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_User_Is_Not_Found()
        {
            var command = new CreateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(), 1, 1000,
                RoomTypeEnumeration.Single, new List<Guid> { Guid.NewGuid() });
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
            };
            var getUserResult = GetResult<User>.Fail(errors);


            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_And_City_Districts_Verification_Fails()
        {
            var command = new CreateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(), 1, 1000,
                RoomTypeEnumeration.Single, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.IncorrectCityDistricts, CityErrorMessage.IncorrectCityDistricts)
            };
            var cityAndCityDistrictsVerificationResult = VerificationResult.Fail(errors);


            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Announcement_Preference_Limit_I_Exceeded()
        {
            var command = new CreateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(), 1, 1000,
                RoomTypeEnumeration.Single, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(UserErrorCodeEnumeration.AnnouncementPreferenceLimitExceeded,
                    UserErrorMessage.AnnouncementPreferenceLimitExceeded)
            };
            var announcementPreferenceLimitIsNotExceededVerificationResult = VerificationResult.Fail(errors);


            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
            _userVerificationServiceMock
                .Setup(x => x.VerifyAnnouncementPreferenceLimitIsNotExceeded(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(announcementPreferenceLimitIsNotExceededVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Flat_For_Rent_Announcement_Preferences_Verification_Fails()
        {
            var command = new CreateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(), 1, 1000,
                RoomTypeEnumeration.Single, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var announcementPreferenceLimitIsNotExceededVerificationResult = VerificationResult.Ok();
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(command.PriceMin)
                .SetPriceMax(command.PriceMax)
                .SetRoomType(command.RoomType)
                .SetCityDistricts(command.CityDistricts)
                .Build();
            var errors = new Collection<IError>
            {
                new Error(RoomForRentAnnouncementPreferenceErrorCode.ExpansibleCityDistricts,
                    RoomForRentAnnouncementPreferenceErrorMessage.ExpansibleCityDistricts)
            };
            var roomForRentAnnouncementPreferencesVerificationResult = VerificationResult.Fail(errors);


            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
            _userVerificationServiceMock
                .Setup(x => x.VerifyAnnouncementPreferenceLimitIsNotExceeded(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(announcementPreferenceLimitIsNotExceededVerificationResult);
            _mapperMock.Setup(x =>
                    x.Map<CreateRoomForRentAnnouncementPreferenceCommand, RoomForRentAnnouncementPreference>(
                        It.IsAny<CreateRoomForRentAnnouncementPreferenceCommand>()))
                .Returns(roomForRentAnnouncementPreference);
            _roomForRentAnnouncementPreferenceVerificationServiceMock
                .Setup(x => x.VerifyRoomForRentAnnouncementPreferences(
                    It.IsAny<IEnumerable<RoomForRentAnnouncementPreference>>()))
                .Returns(roomForRentAnnouncementPreferencesVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}