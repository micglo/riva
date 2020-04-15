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
using Riva.Users.Domain.Users.Repositories;
using Xunit;

namespace Riva.Users.Core.Test.CommandHandlerTests
{
    public class UpdateFlatForRentAnnouncementPreferenceCommandHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<IFlatForRentAnnouncementPreferenceGetterService> _flatForRentAnnouncementPreferenceGetterServiceMock;
        private readonly Mock<IFlatForRentAnnouncementPreferenceVerificationService> _flatForRentAnnouncementPreferenceVerificationServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly ICommandHandler<UpdateFlatForRentAnnouncementPreferenceCommand> _commandHandler;

        public UpdateFlatForRentAnnouncementPreferenceCommandHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _flatForRentAnnouncementPreferenceGetterServiceMock = new Mock<IFlatForRentAnnouncementPreferenceGetterService>();
            _flatForRentAnnouncementPreferenceVerificationServiceMock = new Mock<IFlatForRentAnnouncementPreferenceVerificationService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _commandHandler = new UpdateFlatForRentAnnouncementPreferenceCommandHandler(_userGetterServiceMock.Object,
                _cityVerificationServiceMock.Object, _flatForRentAnnouncementPreferenceGetterServiceMock.Object,
                _userRepositoryMock.Object, _flatForRentAnnouncementPreferenceVerificationServiceMock.Object,
                _communicationBusMock.Object, _integrationEventBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_Flat_For_Rent_Announcement_Preference_When_CityId_Is_Different()
        {
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(command.FlatForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var flatForRentAnnouncementPreferencesVerificationResult = VerificationResult.Ok(); 
            
            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
            _flatForRentAnnouncementPreferenceVerificationServiceMock
                .Setup(x => x.VerifyFlatForRentAnnouncementPreferences(
                    It.IsAny<IEnumerable<FlatForRentAnnouncementPreference>>()))
                .Returns(flatForRentAnnouncementPreferencesVerificationResult);
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
        public async Task HandleAsync_Should_Update_Flat_For_Rent_Announcement_Preference_When_CityId_Is_The_Same_But_City_Districts_Are_Not()
        {
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(command.FlatForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var flatForRentAnnouncementPreferencesVerificationResult = VerificationResult.Ok();

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
            _flatForRentAnnouncementPreferenceVerificationServiceMock
                .Setup(x => x.VerifyFlatForRentAnnouncementPreferences(
                    It.IsAny<IEnumerable<FlatForRentAnnouncementPreference>>()))
                .Returns(flatForRentAnnouncementPreferencesVerificationResult);
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
        public async Task HandleAsync_Should_Update_Flat_For_Rent_Announcement_Preference_When_CityId_And_City_Districts_Are_The_Same()
        {
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(command.FlatForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(command.CityDistricts)
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
            var flatForRentAnnouncementPreferencesVerificationResult = VerificationResult.Ok();

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
            _flatForRentAnnouncementPreferenceVerificationServiceMock
                .Setup(x => x.VerifyFlatForRentAnnouncementPreferences(
                    It.IsAny<IEnumerable<FlatForRentAnnouncementPreference>>()))
                .Returns(flatForRentAnnouncementPreferencesVerificationResult);
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
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
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
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Flat_For_Rent_Announcement_Preference_Is_Not_Found()
        {
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
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
                new Error(FlatForRentAnnouncementPreferenceErrorCode.NotFound,
                    FlatForRentAnnouncementPreferenceErrorMessage.NotFound)
            };
            var getFlatForRentAnnouncementPreferenceResult = GetResult<FlatForRentAnnouncementPreference>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_And_City_Districts_Verification_Fails_For_Different_CityId()
        {
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(command.FlatForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.IncorrectCityDistricts, CityErrorMessage.IncorrectCityDistricts)
            };
            var cityAndCityDistrictsVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
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
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(command.FlatForRentAnnouncementPreferenceId)
                .SetCityId(command.CityId)
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.IncorrectCityDistricts, CityErrorMessage.IncorrectCityDistricts)
            };
            var cityAndCityDistrictsVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Flat_For_Rent_Announcement_Preferences_Verification_Fails()
        {
            var command = new UpdateFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), 1, 3000, 1, 3, new List<Guid> { Guid.NewGuid() });
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(command.FlatForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
            var cityAndCityDistrictsVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(FlatForRentAnnouncementPreferenceErrorCode.ExpansibleCityDistricts,
                    FlatForRentAnnouncementPreferenceErrorMessage.ExpansibleCityDistricts)
            };
            var flatForRentAnnouncementPreferencesVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
            _cityVerificationServiceMock
                .Setup(x => x.VerifyCityAndCityDistrictsAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityAndCityDistrictsVerificationResult);
            _flatForRentAnnouncementPreferenceVerificationServiceMock
                .Setup(x => x.VerifyFlatForRentAnnouncementPreferences(
                    It.IsAny<IEnumerable<FlatForRentAnnouncementPreference>>()))
                .Returns(flatForRentAnnouncementPreferencesVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}