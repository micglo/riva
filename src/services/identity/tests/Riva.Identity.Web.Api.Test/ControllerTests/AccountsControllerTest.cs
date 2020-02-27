using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Queries;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Web.Api.AutoMapperProfiles;
using Riva.Identity.Web.Api.Controllers;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Models.Responses.Accounts;
using Xunit;

namespace Riva.Identity.Web.Api.Test.ControllerTests
{
    public class AccountsControllerTest
    {
        private readonly Mock<IQueryHandler<GetAccountsInputQuery, CollectionOutputQuery<GetAccountsOutputQuery>>> _getAccountsQueryHandlerMock;
        private readonly Mock<IQueryHandler<GetAccountInputQuery, GetAccountOutputQuery>> _getAccountQueryHandlerMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AccountsController _controller;

        public AccountsControllerTest()
        {
            _getAccountsQueryHandlerMock = new Mock<IQueryHandler<GetAccountsInputQuery, CollectionOutputQuery<GetAccountsOutputQuery>>>();
            _getAccountQueryHandlerMock = new Mock<IQueryHandler<GetAccountInputQuery, GetAccountOutputQuery>>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _mapperMock = new Mock<IMapper>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost", 5000);
            httpContext.Request.Path = "/api/accounts";
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _controller = new AccountsController(_getAccountsQueryHandlerMock.Object, _getAccountQueryHandlerMock.Object, 
                _communicationBusMock.Object, _authorizationServiceMock.Object, _mapperMock.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task GetAccountsAsync_Should_Return_OkObjectResult_With_CollectionResponse_With_GetAccountsCollectionItemResponses()
        {
            var getAccountsRequest = new GetAccountsRequest
            {
                Email = "email@email.com",
                Confirmed = true,
                Sort = "email:asc",
                Page = 1,
                PageSize = 100
            };
            var getAccountsInputQuery = new GetAccountsInputQuery(getAccountsRequest.Page, getAccountsRequest.PageSize,
                getAccountsRequest.Sort, getAccountsRequest.Email, getAccountsRequest.Confirmed);
            var getAccountsOutputQueries = new List<GetAccountsOutputQuery>
            {
                new GetAccountsOutputQuery(Guid.NewGuid(), getAccountsRequest.Email, getAccountsRequest.Confirmed.Value,
                    DateTimeOffset.UtcNow, true, null)
            };
            var collectionOutputQuery = new CollectionOutputQuery<GetAccountsOutputQuery>(getAccountsOutputQueries.Count, getAccountsOutputQueries);
            var getAccountsCollectionItemResponses = getAccountsOutputQueries.Select(x =>
                new GetAccountsCollectionItemResponse(x.Id, x.Email, x.Confirmed, x.Created, x.PasswordAssigned, x.LastLogin));
            var collectionResponse = new CollectionResponse<GetAccountsCollectionItemResponse>(getAccountsOutputQueries.Count, getAccountsCollectionItemResponses);

            _mapperMock.Setup(x => x.Map<GetAccountsRequest, GetAccountsInputQuery>(It.IsAny<GetAccountsRequest>()))
                .Returns(getAccountsInputQuery);
            _getAccountsQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetAccountsInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(collectionOutputQuery);
            _mapperMock.Setup(x =>
                x.Map<CollectionOutputQuery<GetAccountsOutputQuery>,
                    CollectionResponse<GetAccountsCollectionItemResponse>>(
                    It.IsAny<CollectionOutputQuery<GetAccountsOutputQuery>>())).Returns(collectionResponse);

            var result = await _controller.GetAccountsAsync(getAccountsRequest);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(collectionResponse);
        }

        [Fact]
        public async Task GetAccountAsync_Should_Return_OkObjectResult_With_GetAccountResponse()
        {
            var authResult = AuthorizationResult.Success();
            var getAccountTokenOutputQueries = new List<GetAccountTokenOutputQuery>
            {
                new GetAccountTokenOutputQuery(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1),
                    TokenTypeEnumeration.AccountConfirmation, "123456")
            };
            var getAccountOutputQuery = new GetAccountOutputQuery(Guid.NewGuid(), "email@email.com", true,
                DateTimeOffset.UtcNow, true, null, new List<Guid> {Guid.NewGuid()}, getAccountTokenOutputQueries);
            var accountTokens = getAccountOutputQuery.Tokens.Select(x =>
                new AccountToken(x.Issued, x.Expires, TokenProfile.ConvertToAccountTokenTypeEnum(x.Type), x.Value));
            var getAccountResponse = new GetAccountResponse(getAccountOutputQuery.Id, getAccountOutputQuery.Email,
                getAccountOutputQuery.Confirmed, getAccountOutputQuery.Created, getAccountOutputQuery.PasswordAssigned,
                getAccountOutputQuery.LastLogin, getAccountOutputQuery.Roles, accountTokens);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);
            _getAccountQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetAccountInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getAccountOutputQuery);
            _mapperMock.Setup(x => x.Map<GetAccountOutputQuery, GetAccountResponse>(It.IsAny<GetAccountOutputQuery>()))
                .Returns(getAccountResponse);

            var result = await _controller.GetAccountAsync(getAccountOutputQuery.Id);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(getAccountResponse);
        }

        [Fact]
        public async Task GetAccountAsync_Should_Return_ObjectResult_With_Forbidden_Status()
        {
            var accountId = Guid.NewGuid();
            var authResult = AuthorizationResult.Failed();

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);

            var result = await _controller.GetAccountAsync(accountId);
            var objectResult = result.As<ObjectResult>();

            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateAccountAsync_Should_Return_AcceptedAtRouteResult()
        {
            var createAccountRequest = new CreateAccountRequest
            {
                Email = "email@email.com",
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };
            var createAccountCommand = new CreateAccountCommand(createAccountRequest.Email, createAccountRequest.Password);

            _mapperMock.Setup(x => x.Map<CreateAccountRequest, CreateAccountCommand>(It.IsAny<CreateAccountRequest>()))
                .Returns(createAccountCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.CreateAccountAsync(createAccountRequest);
            var acceptedAtRouteResult = result.As<AcceptedAtRouteResult>();

            acceptedAtRouteResult.RouteName.Should().BeEquivalentTo("GetAccount");
            acceptedAtRouteResult.RouteValues.Should().BeEquivalentTo(
                new Microsoft.AspNetCore.Routing.RouteValueDictionary(new {id = createAccountCommand.AccountId}));
        }

        [Fact]
        public async Task DeleteAccountAsync_Should_Return_AcceptedResult()
        {
            var accountId = Guid.NewGuid();
            var authResult = AuthorizationResult.Success();

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<DeleteAccountCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteAccountAsync(accountId);
            var acceptedResult = result.As<AcceptedResult>();

            acceptedResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAccountAsync_Should_Return_ObjectResult_With_Forbidden_Status()
        {
            var accountId = Guid.NewGuid();
            var authResult = AuthorizationResult.Failed();

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);

            var result = await _controller.DeleteAccountAsync(accountId);
            var objectResult = result.As<ObjectResult>();

            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateAccountRolesAsync_Should_Return_NoContentResult()
        {
            var accountId = Guid.NewGuid();
            var updateAccountRolesRequest = new UpdateAccountRolesRequest
            {
                Roles = new List<Guid> { Guid.NewGuid() }
            };

            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<UpdateAccountRolesCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateAccountRolesAsync(accountId, updateAccountRolesRequest);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task ConfirmAccountAsync_Should_Return_NoContentResult()
        {
            var confirmAccountRequest = new ConfirmAccountRequest
            {
                Email = "email@email.com",
                Code = "123456"
            };
            var confirmAccountCommand = new ConfirmAccountCommand(confirmAccountRequest.Email, confirmAccountRequest.Code);

            _mapperMock.Setup(x =>
                    x.Map<ConfirmAccountRequest, ConfirmAccountCommand>(It.IsAny<ConfirmAccountRequest>()))
                .Returns(confirmAccountCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<ConfirmAccountCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.ConfirmAccountAsync(confirmAccountRequest);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task RequestAccountConfirmationTokenAsync_Should_Return_NoContentResult()
        {
            var requestAccountConfirmationTokenRequest = new RequestAccountConfirmationTokenRequest
            {
                Email = "email@email.com"
            };
            var requestAccountConfirmationTokenCommand = new RequestAccountConfirmationTokenCommand(requestAccountConfirmationTokenRequest.Email);

            _mapperMock.Setup(x =>
                    x.Map<RequestAccountConfirmationTokenRequest, RequestAccountConfirmationTokenCommand>(
                        It.IsAny<RequestAccountConfirmationTokenRequest>()))
                .Returns(requestAccountConfirmationTokenCommand);
            _communicationBusMock
                .Setup(x => x.SendCommandAsync(It.IsAny<RequestAccountConfirmationTokenCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.RequestAccountConfirmationTokenAsync(requestAccountConfirmationTokenRequest);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task ChangePasswordAsync_Should_Return_NoContentResult()
        {
            var changePasswordRequest = new ChangePasswordRequest
            {
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var authResult = AuthorizationResult.Success();
            var changePasswordCommand = new ChangePasswordCommand(Guid.NewGuid(), changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);
            _mapperMock.Setup(x => x.Map<ChangePasswordRequest, ChangePasswordCommand>(It.IsAny<ChangePasswordRequest>())).Returns(changePasswordCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<ChangePasswordCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.ChangePasswordAsync(changePasswordCommand.AccountId, changePasswordRequest);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task ChangePasswordAsync_Should_Return_ObjectResult_With_Forbidden_Status()
        {
            var changePasswordRequest = new ChangePasswordRequest
            {
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword",
            };
            var authResult = AuthorizationResult.Failed();

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);

            var result = await _controller.ChangePasswordAsync(Guid.NewGuid(), changePasswordRequest);
            var objectResult = result.As<ObjectResult>();

            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AssignPasswordAsync_Should_Return_NoContentResult()
        {
            var assignPasswordRequest = new AssignPasswordRequest
            {
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };
            var authResult = AuthorizationResult.Success();
            var assignPasswordCommand = new AssignPasswordCommand(Guid.NewGuid(), assignPasswordRequest.Password);

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);
            _mapperMock.Setup(x => x.Map<AssignPasswordRequest, AssignPasswordCommand>(It.IsAny<AssignPasswordRequest>()))
                .Returns(assignPasswordCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<AssignPasswordCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.AssignPasswordAsync(assignPasswordCommand.AccountId, assignPasswordRequest);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task AssignPasswordAsync_Should_Return_ObjectResult_With_Forbidden_Status()
        {
            var assignPasswordRequest = new AssignPasswordRequest
            {
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };
            var authResult = AuthorizationResult.Failed();

            _authorizationServiceMock
                .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(authResult);

            var result = await _controller.AssignPasswordAsync(Guid.NewGuid(), assignPasswordRequest);
            var objectResult = result.As<ObjectResult>();

            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task RequestPasswordResetTokenAsync_Should_Return_NoContentResult()
        {
            var requestPasswordResetTokenRequest = new RequestPasswordResetTokenRequest
            {
                Email = "email@email.com"
            };
            var requestPasswordResetTokenCommand = new RequestPasswordResetTokenCommand(requestPasswordResetTokenRequest.Email);

            _mapperMock.Setup(x => x.Map<RequestPasswordResetTokenRequest, RequestPasswordResetTokenCommand>(It.IsAny<RequestPasswordResetTokenRequest>()))
                .Returns(requestPasswordResetTokenCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<RequestPasswordResetTokenCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.RequestPasswordResetTokenAsync(requestPasswordResetTokenRequest);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task ResetPasswordAsync_Should_Return_NoContentResult()
        {
            var resetPasswordRequest = new ResetPasswordRequest
            {
                Email = "email@email.com",
                Code = "123456",
                Password = "Password1234",
                ConfirmPassword = "Password1234",
            };
            var resetPasswordCommand = new ResetPasswordCommand(resetPasswordRequest.Email, resetPasswordRequest.Code, resetPasswordRequest.Password);

            _mapperMock.Setup(x => x.Map<ResetPasswordRequest, ResetPasswordCommand>(It.IsAny<ResetPasswordRequest>())).Returns(resetPasswordCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.ResetPasswordAsync(resetPasswordRequest);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }
    }
}