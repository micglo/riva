using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Queries;
using Riva.Identity.Core.Queries.Handlers;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;
using Xunit;

namespace Riva.Identity.Core.Test.QueryHandlerTests
{
    public class GetAccountQueryHandlerTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetAccountInputQuery, GetAccountOutputQuery> _queryHandler;

        public GetAccountQueryHandlerTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetAccountQueryHandler(_accountGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_GetAccountOutputQuery()
        {
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue("123456")
                .Build();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .SetTokens(new List<Token> { token })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var getAccountTokenOutputQueries =
                account.Tokens.Select(x => new GetAccountTokenOutputQuery(x.Issued, x.Expires, x.Type, x.Value));
            var getAccountOutputQuery = new GetAccountOutputQuery(account.Id, account.Email, account.Confirmed,
                account.Created, !string.IsNullOrWhiteSpace(account.PasswordHash), account.LastLogin, account.Roles,
                getAccountTokenOutputQueries);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _mapperMock.Setup(x => x.Map<Account, GetAccountOutputQuery>(It.IsAny<Account>())).Returns(getAccountOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetAccountInputQuery(account.Id));

            result.Should().BeEquivalentTo(getAccountOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Account_Is_Not_Found()
        {
            var accountId = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);

            Func<Task<GetAccountOutputQuery>> result = async () => await _queryHandler.HandleAsync(new GetAccountInputQuery(accountId));
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}