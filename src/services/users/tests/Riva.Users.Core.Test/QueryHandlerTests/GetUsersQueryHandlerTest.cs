using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Core.Queries;
using Riva.Users.Core.Queries.Handlers;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Repositories;
using Xunit;

namespace Riva.Users.Core.Test.QueryHandlerTests
{
    public class GetUsersQueryHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetUsersInputQuery, CollectionOutputQuery<UserOutputQuery>> _queryHandler;

        public GetUsersQueryHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetUsersQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_UserOutputQuery_When_InputQuery_Is_Null()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var users = new List<User>{ user };
            var userOutputQueries = users.Select(x => new UserOutputQuery(user.Id, user.Email, user.Picture,
                user.ServiceActive, user.AnnouncementPreferenceLimit, user.AnnouncementSendingFrequency,
                new List<RoomForRentAnnouncementPreferenceOutputQuery>(),
                new List<FlatForRentAnnouncementPreferenceOutputQuery>())).ToList();
            var expectedResult = new CollectionOutputQuery<UserOutputQuery>(userOutputQueries.Count(), userOutputQueries);

            _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
            _userRepositoryMock.Setup(x => x.CountAsync()).ReturnsAsync(users.Count);
            _mapperMock.Setup(x => x.Map<List<User>, IEnumerable<UserOutputQuery>>(It.IsAny<List<User>>()))
                .Returns(userOutputQueries);

            var result = await _queryHandler.HandleAsync(null);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_UserOutputQuery_When_InputQuery_Is_Not_Null()
        {
            var inputQuery = new GetUsersInputQuery(1, 1, "email", "email@email.com", true);
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var users = new List<User> { user };
            var userOutputQueries = users.Select(x => new UserOutputQuery(user.Id, user.Email, user.Picture,
                user.ServiceActive, user.AnnouncementPreferenceLimit, user.AnnouncementSendingFrequency,
                new List<RoomForRentAnnouncementPreferenceOutputQuery>(),
                new List<FlatForRentAnnouncementPreferenceOutputQuery>())).ToList();
            var expectedResult = new CollectionOutputQuery<UserOutputQuery>(userOutputQueries.Count(), userOutputQueries);

            _userRepositoryMock.Setup(x => x.FindAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(users);
            _userRepositoryMock.Setup(x => x.CountAsync(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(users.Count);
            _mapperMock.Setup(x => x.Map<List<User>, IEnumerable<UserOutputQuery>>(It.IsAny<List<User>>()))
                .Returns(userOutputQueries);

            var result = await _queryHandler.HandleAsync(inputQuery);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}