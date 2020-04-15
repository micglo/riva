using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Queries;
using Riva.Users.Core.Queries.Handlers;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Entities;
using Xunit;

namespace Riva.Users.Core.Test.QueryHandlerTests
{
    public class GetFlatForRentAnnouncementPreferenceQueryHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IFlatForRentAnnouncementPreferenceGetterService> _flatForRentAnnouncementPreferenceGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetFlatForRentAnnouncementPreferenceInputQuery, FlatForRentAnnouncementPreferenceOutputQuery> _queryHandler;

        public GetFlatForRentAnnouncementPreferenceQueryHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _flatForRentAnnouncementPreferenceGetterServiceMock = new Mock<IFlatForRentAnnouncementPreferenceGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetFlatForRentAnnouncementPreferenceQueryHandler(_userGetterServiceMock.Object,
                _flatForRentAnnouncementPreferenceGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_FlatForRentAnnouncementPreferenceOutputQuery()
        {
            var inputQuery = new GetFlatForRentAnnouncementPreferenceInputQuery(Guid.NewGuid(), Guid.NewGuid());
            var user = User.Builder()
                .SetId(inputQuery.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(inputQuery.FlatForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1000)
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
            var expectedResult = new FlatForRentAnnouncementPreferenceOutputQuery(flatForRentAnnouncementPreference.Id,
                flatForRentAnnouncementPreference.CityId, flatForRentAnnouncementPreference.PriceMin,
                flatForRentAnnouncementPreference.PriceMax, flatForRentAnnouncementPreference.RoomNumbersMin,
                flatForRentAnnouncementPreference.RoomNumbersMax, flatForRentAnnouncementPreference.CityDistricts);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
            _mapperMock.Setup(x =>
                    x.Map<FlatForRentAnnouncementPreference, FlatForRentAnnouncementPreferenceOutputQuery>(
                        It.IsAny<FlatForRentAnnouncementPreference>()))
                .Returns(expectedResult);

            var result = await _queryHandler.HandleAsync(inputQuery);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_User_Is_Not_Found()
        {
            var inputQuery = new GetFlatForRentAnnouncementPreferenceInputQuery(Guid.NewGuid(), Guid.NewGuid());
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
            };
            var getUserResult = GetResult<User>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);

            Func<Task<FlatForRentAnnouncementPreferenceOutputQuery>> result = async () => await _queryHandler.HandleAsync(inputQuery);

            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_FlatForRentAnnouncementPreference_Is_Not_Found()
        {
            var inputQuery = new GetFlatForRentAnnouncementPreferenceInputQuery(Guid.NewGuid(), Guid.NewGuid());
            var user = User.Builder()
                .SetId(inputQuery.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var errors = new Collection<IError>
            {
                new Error(FlatForRentAnnouncementPreferenceErrorCode.NotFound, FlatForRentAnnouncementPreferenceErrorMessage.NotFound)
            };
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);

            Func<Task<FlatForRentAnnouncementPreferenceOutputQuery>> result = async () => await _queryHandler.HandleAsync(inputQuery);

            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}