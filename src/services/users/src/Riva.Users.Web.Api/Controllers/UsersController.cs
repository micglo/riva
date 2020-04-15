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
using Riva.Users.Core.Commands;
using Riva.Users.Core.Extensions;
using Riva.Users.Core.Queries;
using Riva.Users.Web.Api.Models.Requests;
using Riva.Users.Web.Api.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Riva.Users.Web.Api.Controllers
{
    [ApiRoute("users")]
    [ApiVersion("1")]
    [ApiController]
    public class UsersController : RivaControllerBase
    {
        private readonly IQueryHandler<GetUsersInputQuery, CollectionOutputQuery<UserOutputQuery>> _getUsersQueryHandler;
        private readonly IQueryHandler<GetUserInputQuery, UserOutputQuery> _getUserQueryHandler;
        private readonly IQueryHandler<GetFlatForRentAnnouncementPreferenceInputQuery, FlatForRentAnnouncementPreferenceOutputQuery> _getFlatForRentAnnouncementPreferenceQueryHandler;
        private readonly IQueryHandler<GetRoomForRentAnnouncementPreferenceInputQuery, RoomForRentAnnouncementPreferenceOutputQuery> _getRoomForRentAnnouncementPreferenceQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public UsersController(IQueryHandler<GetUsersInputQuery, CollectionOutputQuery<UserOutputQuery>> getUsersQueryHandler, 
            IQueryHandler<GetUserInputQuery, UserOutputQuery> getUserQueryHandler,
            IQueryHandler<GetFlatForRentAnnouncementPreferenceInputQuery, FlatForRentAnnouncementPreferenceOutputQuery> getFlatForRentAnnouncementPreferenceQueryHandler,
            IQueryHandler<GetRoomForRentAnnouncementPreferenceInputQuery, RoomForRentAnnouncementPreferenceOutputQuery> getRoomForRentAnnouncementPreferenceQueryHandler,
            ICommunicationBus communicationBus, IAuthorizationService authorizationService, IMapper mapper)
        {
            _getUsersQueryHandler = getUsersQueryHandler;
            _getUserQueryHandler = getUserQueryHandler;
            _getFlatForRentAnnouncementPreferenceQueryHandler = getFlatForRentAnnouncementPreferenceQueryHandler;
            _getRoomForRentAnnouncementPreferenceQueryHandler = getRoomForRentAnnouncementPreferenceQueryHandler;
            _communicationBus = communicationBus;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpGet("", Name = "GetUsers")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorSystemPolicyName)]
        [SwaggerOperation(
            Summary = "Gets users",
            Description = "Gets users",
            OperationId = "GetUsers",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(200, "Users collection.", typeof(CollectionResponse<UserResponse>))]
        public async Task<IActionResult> GetUsersAsync([FromQuery]GetUsersRequest request)
        {
            var getUsersInputQuery = _mapper.Map<GetUsersRequest, GetUsersInputQuery>(request);
            var collectionOutputQuery = await _getUsersQueryHandler.HandleAsync(getUsersInputQuery);
            var collectionResponse = _mapper.Map<CollectionOutputQuery<UserOutputQuery>, CollectionResponse<UserResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Gets a user",
            Description = "Gets user",
            OperationId = "GetUser",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(200, "User.", typeof(UserResponse))]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> GetUserAsync([FromRoute]Guid id)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var getUserOutputQuery = await _getUserQueryHandler.HandleAsync(new GetUserInputQuery(id));
            var userResponse = _mapper.Map<UserOutputQuery, UserResponse>(getUserOutputQuery);
            return Ok(userResponse);
        }

        [HttpPost("", Name = "CreateUser")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [SwaggerOperation(
            Summary = "Creates a new user",
            Description = "Creates a new user",
            OperationId = "CreateUser",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(201, "User created.", typeof(UserResponse))]
        [SwaggerResponse(409, "User already exist.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserRequest request)
        {
            var createUserCommand = _mapper.Map<CreateUserRequest, CreateUserCommand>(request);
            await _communicationBus.SendCommandAsync(createUserCommand);
            var getUserOutputQuery = await _getUserQueryHandler.HandleAsync(new GetUserInputQuery(request.Id));
            var userResponse = _mapper.Map<UserOutputQuery, UserResponse>(getUserOutputQuery);

            return CreatedAtRoute("GetUser", new { id = userResponse.Id }, userResponse);
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Updates a user",
            Description = "Updates a user",
            OperationId = "UpdateUser",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(202, "User update request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        [SwaggerResponse(409, "User already exist.", typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateUserAsync([FromRoute]Guid id, [FromForm]UpdateUserRequest request)
        {
            ValidatePathIdWithRequestBodyId(id, request.Id);

            var updateUserCommand = _mapper.Map<UpdateUserRequest, UpdateUserCommand>(request);
            await _communicationBus.SendCommandAsync(updateUserCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, updateUserCommand.CorrelationId.ToString());
            return Accepted();
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [SwaggerOperation(
            Summary = "Deletes a user",
            Description = "Deletes a user",
            OperationId = "DeleteUser",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(204, "User deleted.")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute]Guid id)
        {
            var deleteUserCommand = new DeleteUserCommand(id);
            await _communicationBus.SendCommandAsync(deleteUserCommand);
            return NoContent();
        }

        [HttpPost("{id}/flatForRentAnnouncementPreferences", Name = "CreateFlatForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Creates a new flat for rent announcement preference.",
            Description = "Creates a new flat for rent announcement preference.",
            OperationId = "CreateFlatForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(202, "FlatForRentAnnouncementPreference creation request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateFlatForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromBody]CreateFlatForRentAnnouncementPreferenceRequest request)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var createFlatForRentAnnouncementPreferenceCommand = new CreateFlatForRentAnnouncementPreferenceCommand(id,
                request.CityId, request.PriceMin, request.PriceMax, request.RoomNumbersMin, request.RoomNumbersMax,
                request.CityDistricts);
            await _communicationBus.SendCommandAsync(createFlatForRentAnnouncementPreferenceCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, createFlatForRentAnnouncementPreferenceCommand.CorrelationId.ToString());
            return Accepted();
        }

        [HttpGet("{id}/flatForRentAnnouncementPreferences/{flatForRentAnnouncementPreferenceId}", Name = "GetFlatForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Gets a flat for rent announcement preference.",
            Description = "Gets a flat for rent announcement preference.",
            OperationId = "GetFlatForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(200, "FlatForRentAnnouncementPreference.", typeof(FlatForRentAnnouncementPreferenceResponse))]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> GetFlatForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromRoute]Guid flatForRentAnnouncementPreferenceId)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var getFlatForRentAnnouncementPreferenceOutputQuery = await _getFlatForRentAnnouncementPreferenceQueryHandler.HandleAsync(
                new GetFlatForRentAnnouncementPreferenceInputQuery(id, flatForRentAnnouncementPreferenceId));
            var flatForRentAnnouncementPreferenceResponse =
                _mapper.Map<FlatForRentAnnouncementPreferenceOutputQuery, FlatForRentAnnouncementPreferenceResponse>(getFlatForRentAnnouncementPreferenceOutputQuery);
            return Ok(flatForRentAnnouncementPreferenceResponse);
        }

        [HttpPut("{id}/flatForRentAnnouncementPreferences/{flatForRentAnnouncementPreferenceId}", Name = "UpdateFlatForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Updates a flat for rent announcement preference.",
            Description = "Updates a flat for rent announcement preference.",
            OperationId = "UpdateFlatForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(202, "FlatForRentAnnouncementPreference update request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateFlatForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromRoute]Guid flatForRentAnnouncementPreferenceId, 
            [FromBody]UpdateFlatForRentAnnouncementPreferenceRequest request)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var updateFlatForRentAnnouncementPreferenceCommand = new UpdateFlatForRentAnnouncementPreferenceCommand(
                flatForRentAnnouncementPreferenceId, id, request.CityId, request.PriceMin, request.PriceMax,
                request.RoomNumbersMin, request.RoomNumbersMax, request.CityDistricts);
            await _communicationBus.SendCommandAsync(updateFlatForRentAnnouncementPreferenceCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, updateFlatForRentAnnouncementPreferenceCommand.CorrelationId.ToString());
            return Accepted();
        }

        [HttpDelete("{id}/flatForRentAnnouncementPreferences/{flatForRentAnnouncementPreferenceId}", Name = "DeleteFlatForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Deletes a flat for rent announcement preference.",
            Description = "Deletes a flat for rent announcement preference.",
            OperationId = "DeleteFlatForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(202, "FlatForRentAnnouncementPreference deletion request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteFlatForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromRoute]Guid flatForRentAnnouncementPreferenceId)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var deleteFlatForRentAnnouncementPreferenceCommand = new DeleteFlatForRentAnnouncementPreferenceCommand(
                flatForRentAnnouncementPreferenceId, id);
            await _communicationBus.SendCommandAsync(deleteFlatForRentAnnouncementPreferenceCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, deleteFlatForRentAnnouncementPreferenceCommand.CorrelationId.ToString());
            return Accepted();
        }

        [HttpPost("{id}/roomForRentAnnouncementPreferences", Name = "CreateRoomForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Creates a new room for rent announcement preference.",
            Description = "Creates a new room for rent announcement preference.",
            OperationId = "CreateRoomForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(202, "RoomForRentAnnouncementPreference creation request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateRoomForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromBody]CreateRoomForRentAnnouncementPreferenceRequest request)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var createRoomForRentAnnouncementPreferenceCommand = new CreateRoomForRentAnnouncementPreferenceCommand(id,
                request.CityId, request.PriceMin, request.PriceMax, request.RoomType.ConvertToEnumeration(), request.CityDistricts);
            await _communicationBus.SendCommandAsync(createRoomForRentAnnouncementPreferenceCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, createRoomForRentAnnouncementPreferenceCommand.CorrelationId.ToString());
            return Accepted();
        }

        [HttpGet("{id}/roomForRentAnnouncementPreferences/{roomForRentAnnouncementPreferenceId}", Name = "GetRoomForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Gets a room for rent announcement preference.",
            Description = "Gets a room for rent announcement preference.",
            OperationId = "GetRoomForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(200, "RoomForRentAnnouncementPreference.", typeof(RoomForRentAnnouncementPreferenceResponse))]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> GetRoomForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromRoute]Guid roomForRentAnnouncementPreferenceId)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var getRoomForRentAnnouncementPreferenceOutputQuery = await _getRoomForRentAnnouncementPreferenceQueryHandler.HandleAsync(
                new GetRoomForRentAnnouncementPreferenceInputQuery(id, roomForRentAnnouncementPreferenceId));
            var roomForRentAnnouncementPreferenceResponse =
                _mapper.Map<RoomForRentAnnouncementPreferenceOutputQuery, RoomForRentAnnouncementPreferenceResponse>(getRoomForRentAnnouncementPreferenceOutputQuery);
            return Ok(roomForRentAnnouncementPreferenceResponse);
        }

        [HttpPut("{id}/roomForRentAnnouncementPreferences/{roomForRentAnnouncementPreferenceId}", Name = "UpdateRoomForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Updates a room for rent announcement preference.",
            Description = "Updates a room for rent announcement preference.",
            OperationId = "UpdateRoomForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(202, "RoomForRentAnnouncementPreference update request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateRoomForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromRoute]Guid roomForRentAnnouncementPreferenceId,
            [FromBody]UpdateRoomForRentAnnouncementPreferenceRequest request)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var updateRoomForRentAnnouncementPreferenceCommand = new UpdateRoomForRentAnnouncementPreferenceCommand(
                roomForRentAnnouncementPreferenceId, id, request.CityId, request.PriceMin, request.PriceMax,
                request.RoomType.ConvertToEnumeration(), request.CityDistricts);
            await _communicationBus.SendCommandAsync(updateRoomForRentAnnouncementPreferenceCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, updateRoomForRentAnnouncementPreferenceCommand.CorrelationId.ToString());
            return Accepted();
        }

        [HttpDelete("{id}/roomForRentAnnouncementPreferences/{roomForRentAnnouncementPreferenceId}", Name = "DeleteRoomForRentAnnouncementPreference")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserPolicyName)]
        [SwaggerOperation(
            Summary = "Deletes a room for rent announcement preference.",
            Description = "Deletes a room for rent announcement preference.",
            OperationId = "DeleteRoomForRentAnnouncementPreference",
            Tags = new[] { "Users" }
        )]
        [SwaggerResponse(202, "RoomForRentAnnouncementPreference deletion request accepted.")]
        [SwaggerResponse(403, "Forbidden.", typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteRoomForRentAnnouncementPreferenceAsync([FromRoute]Guid id, [FromRoute]Guid roomForRentAnnouncementPreferenceId)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, ResourceOwnerPolicy.ResourceOwnerPolicyName);
            if (!authResult.Succeeded)
                return CreateErrorResult(HttpStatusCode.Forbidden);

            var deleteRoomForRentAnnouncementPreferenceCommand = new DeleteRoomForRentAnnouncementPreferenceCommand(
                roomForRentAnnouncementPreferenceId, id);
            await _communicationBus.SendCommandAsync(deleteRoomForRentAnnouncementPreferenceCommand);

            Response.Headers.Add(HeaderNameConstants.XCorrelationId, deleteRoomForRentAnnouncementPreferenceCommand.CorrelationId.ToString());
            return Accepted();
        }
    }
}