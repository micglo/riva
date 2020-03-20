using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Commands.Handlers;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Announcements.Core.Test.CommandHandlerTests
{
    public class CreateFlatForRentAnnouncementCommandHandlerTest
    {
        private readonly Mock<IFlatForRentAnnouncementRepository> _flatForRentAnnouncementRepositoryMock;
        private readonly Mock<ICityVerificationService> _cityVerificationServiceMock;
        private readonly Mock<ICityDistrictVerificationService> _cityDistrictVerificationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICommandHandler<CreateFlatForRentAnnouncementCommand> _commandHandler;

        public CreateFlatForRentAnnouncementCommandHandlerTest()
        {
            _flatForRentAnnouncementRepositoryMock = new Mock<IFlatForRentAnnouncementRepository>();
            _cityVerificationServiceMock = new Mock<ICityVerificationService>();
            _cityDistrictVerificationServiceMock = new Mock<ICityDistrictVerificationService>();
            _mapperMock = new Mock<IMapper>();
            _commandHandler = new CreateFlatForRentAnnouncementCommandHandler(
                _flatForRentAnnouncementRepositoryMock.Object, _cityVerificationServiceMock.Object,
                _cityDistrictVerificationServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_FlatForRentAnnouncement()
        {
            var cityVerificationResult = VerificationResult.Ok();
            var cityDistrictsVerificationResult = VerificationResult.Ok();
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var createFlatForRentAnnouncementCommand = new CreateFlatForRentAnnouncementCommand(
                flatForRentAnnouncement.Id, flatForRentAnnouncement.Title, flatForRentAnnouncement.SourceUrl,
                flatForRentAnnouncement.CityId, flatForRentAnnouncement.Description, flatForRentAnnouncement.Price,
                flatForRentAnnouncement.NumberOfRooms, flatForRentAnnouncement.CityDistricts);

            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);
            _mapperMock.Setup(x =>
                x.Map<CreateFlatForRentAnnouncementCommand, FlatForRentAnnouncement>(
                    It.IsAny<CreateFlatForRentAnnouncementCommand>())).Returns(flatForRentAnnouncement);
            _flatForRentAnnouncementRepositoryMock.Setup(x => x.AddAsync(It.IsAny<FlatForRentAnnouncement>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createFlatForRentAnnouncementCommand);

            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_City_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var cityVerificationResult = VerificationResult.Fail(errors);
            var createFlatForRentAnnouncementCommand = new CreateFlatForRentAnnouncementCommand(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), "Description", null, null, new List<Guid>());

            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createFlatForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Any_Of_CityDistricts_Is_Not_Found()
        {
            var cityVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var cityDistrictsVerificationResult = VerificationResult.Fail(errors);
            var createFlatForRentAnnouncementCommand = new CreateFlatForRentAnnouncementCommand(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), "Description", null, null, new List<Guid>());

            _cityVerificationServiceMock.Setup(x => x.VerifyCityExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityVerificationResult);
            _cityDistrictVerificationServiceMock
                .Setup(x => x.VerifyCityDistrictsExistAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistrictsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(createFlatForRentAnnouncementCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}