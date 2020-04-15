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
using Riva.Users.Domain.Users.Enumerations;
using Xunit;

namespace Riva.Users.Core.Test.QueryHandlerTests
{
    public class GetRoomForRentAnnouncementPreferenceQueryHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IRoomForRentAnnouncementPreferenceGetterService> _roomForRentAnnouncementPreferenceGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetRoomForRentAnnouncementPreferenceInputQuery, RoomForRentAnnouncementPreferenceOutputQuery> _queryHandler;

        public GetRoomForRentAnnouncementPreferenceQueryHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _roomForRentAnnouncementPreferenceGetterServiceMock = new Mock<IRoomForRentAnnouncementPreferenceGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetRoomForRentAnnouncementPreferenceQueryHandler(_userGetterServiceMock.Object,
                _roomForRentAnnouncementPreferenceGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_RoomForRentAnnouncementPreferenceOutputQuery()
        {
            var inputQuery = new GetRoomForRentAnnouncementPreferenceInputQuery(Guid.NewGuid(), Guid.NewGuid());
            var user = User.Builder()
                .SetId(inputQuery.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(inputQuery.RoomForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .Build();
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
            var expectedResult = new RoomForRentAnnouncementPreferenceOutputQuery(roomForRentAnnouncementPreference.Id,
                roomForRentAnnouncementPreference.CityId, roomForRentAnnouncementPreference.PriceMin,
                roomForRentAnnouncementPreference.PriceMax, roomForRentAnnouncementPreference.RoomType,
                roomForRentAnnouncementPreference.CityDistricts);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);
            _mapperMock.Setup(x =>
                    x.Map<RoomForRentAnnouncementPreference, RoomForRentAnnouncementPreferenceOutputQuery>(
                        It.IsAny<RoomForRentAnnouncementPreference>()))
                .Returns(expectedResult);

            var result = await _queryHandler.HandleAsync(inputQuery);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_User_Is_Not_Found()
        {
            var inputQuery = new GetRoomForRentAnnouncementPreferenceInputQuery(Guid.NewGuid(), Guid.NewGuid());
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
            };
            var getUserResult = GetResult<User>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);

            Func<Task<RoomForRentAnnouncementPreferenceOutputQuery>> result = async () => await _queryHandler.HandleAsync(inputQuery);

            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_RoomForRentAnnouncementPreference_Is_Not_Found()
        {
            var inputQuery = new GetRoomForRentAnnouncementPreferenceInputQuery(Guid.NewGuid(), Guid.NewGuid());
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
                new Error(RoomForRentAnnouncementPreferenceErrorCode.NotFound, RoomForRentAnnouncementPreferenceErrorMessage.NotFound)
            };
            var getRoomForRentAnnouncementPreferenceResult =
                GetResult<RoomForRentAnnouncementPreference>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _roomForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getRoomForRentAnnouncementPreferenceResult);

            Func<Task<RoomForRentAnnouncementPreferenceOutputQuery>> result = async () => await _queryHandler.HandleAsync(inputQuery);

            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}