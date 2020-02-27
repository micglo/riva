using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Attributes.ActionFilters;
using Riva.BuildingBlocks.WebApi.Attributes.Routes;
using Riva.BuildingBlocks.WebApi.Controllers;
using Riva.BuildingBlocks.WebApi.Models.Errors;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Queries;
using Riva.Identity.Web.Api.Models.Requests.Roles;
using Riva.Identity.Web.Api.Models.Responses.Roles;
using Swashbuckle.AspNetCore.Annotations;

namespace Riva.Identity.Web.Api.Controllers
{
    [ApiRoute("roles")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
    public class RolesController : RivaControllerBase
    {
        private readonly IQueryHandler<GetRolesInputQuery, CollectionOutputQuery<RoleOutputQuery>> _getRolesQueryHandler;
        private readonly IQueryHandler<GetRoleInputQuery, RoleOutputQuery> _getRoleQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IMapper _mapper;

        public RolesController(IQueryHandler<GetRolesInputQuery, CollectionOutputQuery<RoleOutputQuery>> getRolesQueryHandler, 
            IQueryHandler<GetRoleInputQuery, RoleOutputQuery> getRoleQueryHandler, ICommunicationBus communicationBus, IMapper mapper)
        {
            _getRolesQueryHandler = getRolesQueryHandler;
            _getRoleQueryHandler = getRoleQueryHandler;
            _communicationBus = communicationBus;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetRoles")]
        [SwaggerOperation(
            Summary = "Gets roles",
            Description = "Gets roles",
            OperationId = "GetRoles",
            Tags = new[] { "Roles" }
        )]
        [SwaggerResponse(200, "Roles collection.", typeof(CollectionResponse<GetRoleResponse>))]
        public async Task<IActionResult> GetRolesAsync()
        {
            var collectionOutputQuery = await _getRolesQueryHandler.HandleAsync(new GetRolesInputQuery());
            var collectionResponse = _mapper.Map<CollectionOutputQuery<RoleOutputQuery>, CollectionResponse<GetRoleResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetRole")]
        [SwaggerOperation(
            Summary = "Gets a role",
            Description = "Gets a role",
            OperationId = "GetRole",
            Tags = new[] { "Roles" }
        )]
        [SwaggerResponse(200, "Role.", typeof(GetRoleResponse))]
        public async Task<IActionResult> GetRoleAsync([FromRoute]Guid id)
        {
            var getRoleOutputQuery = await _getRoleQueryHandler.HandleAsync(new GetRoleInputQuery(id));
            var getRoleResponse = _mapper.Map<RoleOutputQuery, GetRoleResponse>(getRoleOutputQuery);
            return Ok(getRoleResponse);
        }

        [HttpPost("", Name = "CreateRole")]
        [SwaggerOperation(
            Summary = "Creates a new role",
            Description = "Creates a new role",
            OperationId = "CreateRole",
            Tags = new[] { "Roles" }
        )]
        [SwaggerResponse(201, "Role created.", typeof(GetRoleResponse))]
        [SwaggerResponse(409, "Role already exist.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateRoleAsync([FromBody]CreateRoleRequest request)
        {
            var createRoleCommand = _mapper.Map<CreateRoleRequest, CreateRoleCommand>(request);
            await _communicationBus.SendCommandAsync(createRoleCommand);
            var getRoleOutputQuery = await _getRoleQueryHandler.HandleAsync(new GetRoleInputQuery(createRoleCommand.RoleId));
            var getRoleResponse = _mapper.Map<RoleOutputQuery, GetRoleResponse>(getRoleOutputQuery);

            return CreatedAtRoute("GetRole", new { id = getRoleResponse.Id }, getRoleResponse);
        }

        [HttpPut("{id}", Name = "UpdateRole")]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Updates a role",
            Description = "Updates a role",
            OperationId = "UpdateRole",
            Tags = new[] { "Roles" }
        )]
        [SwaggerResponse(200, "Role updated", typeof(GetRoleResponse))]
        public async Task<IActionResult> UpdateRoleAsync([FromRoute]Guid id, [FromBody]UpdateRoleRequest request, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            ValidatePathIdWithRequestBodyId(id, request.Id);

            request.SetRowVersion(rowVersion);
            var updateRoleCommand = _mapper.Map<UpdateRoleRequest, UpdateRoleCommand>(request);
            await _communicationBus.SendCommandAsync(updateRoleCommand);
            var getRoleOutputQuery = await _getRoleQueryHandler.HandleAsync(new GetRoleInputQuery(id));
            var getRoleResponse = _mapper.Map<RoleOutputQuery, GetRoleResponse>(getRoleOutputQuery);

            return Ok(getRoleResponse);
        }

        [HttpDelete("{id}", Name = "DeleteRole")]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Deletes a role",
            Description = "Deletes a role",
            OperationId = "DeleteRole",
            Tags = new[] { "Roles" }
        )]
        [SwaggerResponse(204, "Role deleted.")]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute]Guid id, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            var deleteRoleCommand = new DeleteRoleCommand(id, rowVersion);
            await _communicationBus.SendCommandAsync(deleteRoleCommand);
            return NoContent();
        }
    }
}