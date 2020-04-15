using System;
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
    public class UpdateRoomForRentAnnouncementPreferenceCommandHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<IRoomForRentAnnouncementPreferenceGetterService> _roomForRentAnnouncementPreferenceGetterServiceMock;
        private readonly Mock<IRoomForRentAnnouncementPreferenceVerificationService> _roomForRentAnnouncementPreferenceVerificationServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly ICommandHandler<UpdateRoomForRentAnnouncementPreferenceCommand> _commandHandler;

        public UpdateRoomForRentAnnouncementPreferenceCommandHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _roomForRentAnnouncementPreferenceGetterServiceMock = new Mock<IRoomForRentAnnouncementPreferenceGetterService>();
            _roomForRentAnnouncementPreferenceVerificationServiceMock = new Mock<IRoomForRentAnnouncementPreferenceVerificationService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _commandHandler = new UpdateRoomForRentAnnouncementPreferenceCommandHandler(_userGetterServiceMock.Object,
                _cityVerificationServiceMock.Object, _roomForRentAnnouncementPreferenceGetterServiceMock.Object,
                _userRepositoryMock.Object, _roomForRentAnnouncementPreferenceVerificationServiceMock.Object,
                _communicationBusMock.Object, _integrationEventBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_Room_For_Rent_Announcement_Preference_When_CityId_Is_Different()
        {
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var roomForRentAnnouncementPreferencesVerificationResult = VerificationResult.Ok();

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
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
        public async Task HandleAsync_Should_Update_Room_For_Rent_Announcement_Preference_When_CityId_Is_The_Same_But_City_Districts_Are_Not()
        {
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var roomForRentAnnouncementPreferencesVerificationResult = VerificationResult.Ok();

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
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
        public async Task HandleAsync_Should_Update_Room_For_Rent_Announcement_Preference_When_CityId_And_City_Districts_Are_The_Same()
        {
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(command.CityDistricts)
                .Build();
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
            var roomForRentAnnouncementPreferencesVerificationResult = VerificationResult.Ok();

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);
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
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
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
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Room_For_Rent_Announcement_Preference_Is_Not_Found()
        {
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
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
                new Error(RoomForRentAnnouncementPreferenceErrorCode.NotFound,
                    RoomForRentAnnouncementPreferenceErrorMessage.NotFound)
            };
            var getRoomForRentAnnouncementPreferenceResult = GetResult<RoomForRentAnnouncementPreference>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_And_City_Districts_Verification_Fails_For_Different_CityId()
        {
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.IncorrectCityDistricts, CityErrorMessage.IncorrectCityDistricts)
            };
            var cityAndCityDistrictsVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_And_City_Districts_Verification_Fails_For_The_Same_CityId()
        {
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.IncorrectCityDistricts, CityErrorMessage.IncorrectCityDistricts)
            };
            var cityAndCityDistrictsVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Room_For_Rent_Announcement_Preferences_Verification_Fails()
        {
            var command = new UpdateRoomForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 1500, RoomTypeEnumeration.Double, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(command.RoomForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(RoomForRentAnnouncementPreferenceErrorCode.ExpansibleCityDistricts,
                    RoomForRentAnnouncementPreferenceErrorMessage.ExpansibleCityDistricts)
            };
            var roomForRentAnnouncementPreferencesVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
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