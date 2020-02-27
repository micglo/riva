using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Core.Queries;
using Riva.Identity.Core.Queries.Handlers;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.QueryHandlerTests
{
    public class GetAccountsQueryHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetAccountsInputQuery, CollectionOutputQuery<GetAccountsOutputQuery>> _queryHandler;

        public GetAccountsQueryHandlerTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetAccountsQueryHandler(_accountRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_GetAccountsOutputQuery_When_Input_Is_Not_Null()
        {
            var getAccountsInputQuery = new GetAccountsInputQuery(1, 100, "email:asc", "email@email.com", true);
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(getAccountsInputQuery.Email)
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var accounts = new List<Account> { account };
            var getAccountsOutputQueries = accounts
                .Select(x => new GetAccountsOutputQuery(x.Id, x.Email, x.Confirmed, x.Created,
                    !string.IsNullOrWhiteSpace(x.PasswordHash), x.LastLogin)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<GetAccountsOutputQuery>(getAccountsOutputQueries.Count, getAccountsOutputQueries);

            _accountRepositoryMock.Setup(x => x.FindAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(accounts);
            _accountRepositoryMock.Setup(x => x.CountAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(accounts.Count);
            _mapperMock.Setup(x => x.Map<List<Account>, IEnumerable<GetAccountsOutputQuery>>(It.IsAny<List<Account>>()))
                .Returns(getAccountsOutputQueries);

            var result = await _queryHandler.HandleAsync(getAccountsInputQuery);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_GetAccountsOutputQuery_When_Input_Is_Null()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var accounts = new List<Account> { account };
            var getAccountsOutputQueries = accounts
                .Select(x => new GetAccountsOutputQuery(x.Id, x.Email, x.Confirmed, x.Created,
                    !string.IsNullOrWhiteSpace(x.PasswordHash), x.LastLogin)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<GetAccountsOutputQuery>(getAccountsOutputQueries.Count, getAccountsOutputQueries);

            _accountRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(accounts);
            _accountRepositoryMock.Setup(x => x.CountAsync()).ReturnsAsync(accounts.Count);
            _mapperMock.Setup(x => x.Map<List<Account>, IEnumerable<GetAccountsOutputQuery>>(It.IsAny<List<Account>>()))
                .Returns(getAccountsOutputQueries);

            var result = await _queryHandler.HandleAsync(null);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }
    }
}