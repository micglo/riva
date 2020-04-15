using System;
using System.Collections.Generic;
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
using Xunit;

namespace Riva.Users.Core.Test.QueryHandlerTests
{
    public class GetUserQueryHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetUserInputQuery, UserOutputQuery> _queryHandler;

        public GetUserQueryHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetUserQueryHandler(_userGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_UserOutputQuery()
        {
            var inputQuery = new GetUserInputQuery(Guid.NewGuid());
            var user = User.Builder()
                .SetId(inputQuery.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var expectedResult = new UserOutputQuery(user.Id, user.Email, user.Picture, user.ServiceActive,
                user.AnnouncementPreferenceLimit, user.AnnouncementSendingFrequency,
                new List<RoomForRentAnnouncementPreferenceOutputQuery>(),
                new List<FlatForRentAnnouncementPreferenceOutputQuery>());

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _mapperMock.Setup(x => x.Map<User, UserOutputQuery>(It.IsAny<User>())).Returns(expectedResult);

            var result = await _queryHandler.HandleAsync(inputQuery);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_User_Is_Not_Found()
        {
            var inputQuery = new GetUserInputQuery(Guid.NewGuid());
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
            };
            var getUserResult = GetResult<User>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);

            Func<Task<UserOutputQuery>> result = async () => await _queryHandler.HandleAsync(inputQuery);

            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}