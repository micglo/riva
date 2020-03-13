using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Web.Api.Models.Requests;
using Riva.AdministrativeDivisions.Web.Api.Models.Responses;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Attributes.ActionFilters;
using Riva.BuildingBlocks.WebApi.Attributes.Routes;
using Riva.BuildingBlocks.WebApi.Controllers;
using Riva.BuildingBlocks.WebApi.Models.Errors;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Swashbuckle.AspNetCore.Annotations;

namespace Riva.AdministrativeDivisions.Web.Api.Controllers
{
    [ApiRoute("states")]
    [ApiVersion("1")]
    [ApiController]
    public class StatesController : RivaControllerBase
    {
        private readonly IQueryHandler<GetStatesInputQuery, CollectionOutputQuery<StateOutputQuery>> _getStatesQueryHandler;
        private readonly IQueryHandler<GetStateInputQuery, StateOutputQuery> _getStateQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IMapper _mapper;

        public StatesController(IQueryHandler<GetStatesInputQuery, CollectionOutputQuery<StateOutputQuery>> getStatesQueryHandler, 
            IQueryHandler<GetStateInputQuery, StateOutputQuery> getStateQueryHandler, ICommunicationBus communicationBus, IMapper mapper)
        {
            _getStatesQueryHandler = getStatesQueryHandler;
            _getStateQueryHandler = getStateQueryHandler;
            _communicationBus = communicationBus;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetStates")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserSystemPolicyName)]
        [SwaggerOperation(
            Summary = "Gets states",
            Description = "Gets states",
            OperationId = "GetStates",
            Tags = new[] { "States" }
        )]
        [SwaggerResponse(200, "States collection.", typeof(CollectionResponse<StateResponse>))]
        public async Task<IActionResult> GetStatesAsync([FromQuery]GetStatesRequest request)
        {
            var getStatesInputQuery = _mapper.Map<GetStatesRequest, GetStatesInputQuery>(request);
            var collectionOutputQuery = await _getStatesQueryHandler.HandleAsync(getStatesInputQuery);
            var collectionResponse = _mapper.Map<CollectionOutputQuery<StateOutputQuery>, CollectionResponse<StateResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetState")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserSystemPolicyName)]
        [SwaggerOperation(
            Summary = "Gets a state",
            Description = "Gets a state",
            OperationId = "GetState",
            Tags = new[] { "States" }
        )]
        [SwaggerResponse(200, "State.", typeof(StateResponse))]
        public async Task<IActionResult> GetStateAsync([FromRoute]Guid id)
        {
            var getStateOutputQuery = await _getStateQueryHandler.HandleAsync(new GetStateInputQuery(id));
            var stateResponse = _mapper.Map<StateOutputQuery, StateResponse>(getStateOutputQuery);
            return Ok(stateResponse);
        }

        [HttpPost("", Name = "CreateState")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [SwaggerOperation(
            Summary = "Creates a new state",
            Description = "Creates a new state",
            OperationId = "CreateState",
            Tags = new[] { "States" }
        )]
        [SwaggerResponse(201, "State created.", typeof(StateResponse))]
        [SwaggerResponse(409, "State already exists.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateStateAsync([FromBody]CreateStateRequest request)
        {
            var createStateCommand = _mapper.Map<CreateStateRequest, CreateStateCommand>(request);
            await _communicationBus.SendCommandAsync(createStateCommand);
            var getStateOutputQuery = await _getStateQueryHandler.HandleAsync(new GetStateInputQuery(createStateCommand.StateId));
            var stateResponse = _mapper.Map<StateOutputQuery, StateResponse>(getStateOutputQuery);

            return CreatedAtRoute("GetState", new { id = stateResponse.Id }, stateResponse);
        }

        [HttpPut("{id}", Name = "UpdateState")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Updates a state",
            Description = "Updates a state",
            OperationId = "UpdateState",
            Tags = new[] { "States" }
        )]
        [SwaggerResponse(200, "State updated", typeof(StateResponse))]
        public async Task<IActionResult> UpdateStateAsync([FromRoute]Guid id, [FromBody]UpdateStateRequest request, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            ValidatePathIdWithRequestBodyId(id, request.Id);

            request.SetRowVersion(rowVersion);
            var updateStateCommand = _mapper.Map<UpdateStateRequest, UpdateStateCommand>(request);
            await _communicationBus.SendCommandAsync(updateStateCommand);
            var getStateOutputQuery = await _getStateQueryHandler.HandleAsync(new GetStateInputQuery(updateStateCommand.StateId));
            var stateResponse = _mapper.Map<StateOutputQuery, StateResponse>(getStateOutputQuery);

            return Ok(stateResponse);
        }

        [HttpDelete("{id}", Name = "DeleteState")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Deletes a state",
            Description = "Deletes a state",
            OperationId = "DeleteState",
            Tags = new[] { "States" }
        )]
        [SwaggerResponse(204, "State deleted.")]
        public async Task<IActionResult> DeleteStateAsync([FromRoute]Guid id, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            var deleteStateCommand = new DeleteStateCommand(id, rowVersion);
            await _communicationBus.SendCommandAsync(deleteStateCommand);

            return NoContent();
        }
    }
}