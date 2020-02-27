using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Attributes.Routes;
using Riva.BuildingBlocks.WebApi.Authorization.Policies;
using Riva.BuildingBlocks.WebApi.Controllers;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.Models.Errors;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Queries;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Models.Responses.Accounts;
using Swashbuckle.AspNetCore.Annotations;

namespace Riva.Identity.Web.Api.Controllers
{
    [ApiRoute("accounts")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
    public class AccountsController : RivaControllerBase
    {
        private readonly IQueryHandler<GetAccountsInputQuery, CollectionOutputQuery<GetAccountsOutputQuery>> _getAccountsQueryHandler;
        private readonly IQueryHandler<GetAccountInputQuery, GetAccountOutputQuery> _getAccountQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public AccountsController(IQueryHandler<GetAccountsInputQuery, CollectionOutputQuery<GetAccountsOutputQuery>> getAccountsQueryHandler,
            IQueryHandler<GetAccountInputQuery, GetAccountOutputQuery> getAccountQueryHandler, ICommunicationBus communicationBus,
            IAuthorizationService authorizationService, IMapper mapper)
        {
            _getAccountsQueryHandler = getAccountsQueryHandler;
            _getAccountQueryHandler = getAccountQueryHandler;
            _communicationBus = communicationBus;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetAccounts")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [SwaggerOperation(
            Summary = "Gets accounts",
            Description = "Gets accounts",
            OperationId = "GetAccounts",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(200, "Accounts collection.", typeof(CollectionResponse<GetAccountsCollectionItemResponse>))]
        public async Task<IActionResult> GetAccountsAsync([FromQuery]GetAccountsRequest request)
        {
            var getAccountsInputQuery = _mapper.Map<GetAccountsRequest, GetAccountsInputQuery>(request);
            var collectionOutputQuery = await _getAccountsQueryHandler.HandleAsync(getAccountsInputQuery);
            var collectionResponse = _mapper.Map<CollectionOutputQuery<GetAccountsOutputQuery>, CollectionResponse<GetAccountsCollectionItemResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetAccount")]
        [SwaggerOperation(
            Summary = "Gets an account",
            Description = "Gets an account",
            OperationId = "GetAccount",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(200, "Account.", typeof(GetAccountResponse))]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAccountAsync([FromRoute]Guid id)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var getAccountOutputQuery = await _getAccountQueryHandler.HandleAsync(new GetAccountInputQuery(id));
            var getAccountResponse = _mapper.Map<GetAccountOutputQuery, GetAccountResponse>(getAccountOutputQuery);
            return Ok(getAccountResponse);
        }

        [AllowAnonymous]
        [HttpPost("", Name = "CreateAccount")]
        [SwaggerOperation(
            Summary = "Creates an new account",
            Description = "Creates an new account",
            OperationId = "CreateAccount",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(202, "Account creation request accepted.")]
        [SwaggerResponse(409, "Account already exist.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAccountAsync([FromBody]CreateAccountRequest request)
        {
            var createAccountCommand = _mapper.Map<CreateAccountRequest, CreateAccountCommand>(request);
            await _communicationBus.SendCommandAsync(createAccountCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, createAccountCommand.CorrelationId.ToString());
            return AcceptedAtRoute("GetAccount", new { id = createAccountCommand.AccountId });
        }

        [HttpDelete("{id}", Name = "DeleteAccount")]
        [SwaggerOperation(
            Summary = "Deletes an account",
            Description = "Deletes an account",
            OperationId = "DeleteAccount",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(202, "Account deletion request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteAccountAsync([FromRoute]Guid id)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var deleteAccountCommand = new DeleteAccountCommand(id);
            await _communicationBus.SendCommandAsync(deleteAccountCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, deleteAccountCommand.CorrelationId.ToString());
            return Accepted();
        }

        [HttpPut("{id}/roles", Name = "UpdateAccountRoles")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [SwaggerOperation(
            Summary = "Updates account roles",
            Description = "Updates account roles",
            OperationId = "UpdateAccountRoles",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(204, "Account roles updated.")]
        public async Task<IActionResult> UpdateAccountRolesAsync([FromRoute]Guid id, [FromBody]UpdateAccountRolesRequest request)
        {
            var updateAccountRolesCommand = new UpdateAccountRolesCommand(id, request.Roles);
            await _communicationBus.SendCommandAsync(updateAccountRolesCommand);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("confirmations", Name = "ConfirmAccount")]
        [SwaggerOperation(
            Summary = "Confirms an account",
            Description = "Confirms an account",
            OperationId = "ConfirmAccount",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(204, "Account confirmed.")]
        public async Task<IActionResult> ConfirmAccountAsync([FromBody]ConfirmAccountRequest request)
        {
            var confirmAccountCommand = _mapper.Map<ConfirmAccountRequest, ConfirmAccountCommand>(request);
            await _communicationBus.SendCommandAsync(confirmAccountCommand);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("confirmations/tokens", Name = "RequestAccountConfirmationToken")]
        [SwaggerOperation(
            Summary = "Requests an account confirmation token",
            Description = "Requests an account confirmation token",
            OperationId = "RequestAccountConfirmationToken",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(204, "Account confirmation token sent.")]
        public async Task<IActionResult> RequestAccountConfirmationTokenAsync([FromBody]RequestAccountConfirmationTokenRequest request)
        {
            var requestAccountConfirmationTokenCommand = _mapper.Map<RequestAccountConfirmationTokenRequest, RequestAccountConfirmationTokenCommand>(request);
            await _communicationBus.SendCommandAsync(requestAccountConfirmationTokenCommand);
            return NoContent();
        }

        [HttpPost("{id}/passwords/changes", Name = "ChangePassword")]
        [SwaggerOperation(
            Summary = "Changes a password",
            Description = "Changes a password",
            OperationId = "ChangePassword",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(204, "Password changed.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> ChangePasswordAsync([FromRoute]Guid id, [FromBody]ChangePasswordRequest request)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var changePasswordCommand = new ChangePasswordCommand(id, request.OldPassword, request.NewPassword);
            await _communicationBus.SendCommandAsync(changePasswordCommand);
            return NoContent();
        }

        [HttpPost("{id}/passwords/assignments", Name = "AssignPassword")]
        [SwaggerOperation(
            Summary = "Assignes a password",
            Description = "Assignes a password",
            OperationId = "AssignPassword",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(204, "Password assigned.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> AssignPasswordAsync([FromRoute]Guid id, [FromBody]AssignPasswordRequest request)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var assignPasswordCommand = new AssignPasswordCommand(id, request.Password);
            await _communicationBus.SendCommandAsync(assignPasswordCommand);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("passwords/tokens", Name = "RequestPasswordResetToken")]
        [SwaggerOperation(
            Summary = "Requests password reset token",
            Description = "Requests password reset token",
            OperationId = "RequestPasswordResetToken",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(204, "Password reset token sent.")]
        public async Task<IActionResult> RequestPasswordResetTokenAsync([FromBody]RequestPasswordResetTokenRequest request)
        {
            var requestPasswordResetTokenCommand = _mapper.Map<RequestPasswordResetTokenRequest, RequestPasswordResetTokenCommand>(request);
            await _communicationBus.SendCommandAsync(requestPasswordResetTokenCommand);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("passwords/resets", Name = "ResetPassword")]
        [SwaggerOperation(
            Summary = "Resets a password",
            Description = "Resets a password",
            OperationId = "ResetPassword",
            Tags = new[] { "Accounts" }
        )]
        [SwaggerResponse(204, "Password reseted.")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody]ResetPasswordRequest request)
        {
            var resetPasswordCommand = _mapper.Map<ResetPasswordRequest, ResetPasswordCommand>(request);
            await _communicationBus.SendCommandAsync(resetPasswordCommand);
            return NoContent();
        }
    }
}