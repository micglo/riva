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
    [ApiRoute("cities")]
    [ApiVersion("1")]
    [ApiController]
    public class CitiesController : RivaControllerBase
    {
        private readonly IQueryHandler<GetCitiesInputQuery, CollectionOutputQuery<CityOutputQuery>> _getCitiesQueryHandler;
        private readonly IQueryHandler<GetCityInputQuery, CityOutputQuery> _getCityQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IMapper _mapper;

        public CitiesController(IQueryHandler<GetCitiesInputQuery, CollectionOutputQuery<CityOutputQuery>> getCitiesQueryHandler,
            IQueryHandler<GetCityInputQuery, CityOutputQuery> getCityQueryHandler, ICommunicationBus communicationBus, IMapper mapper)
        {
            _getCitiesQueryHandler = getCitiesQueryHandler;
            _getCityQueryHandler = getCityQueryHandler;
            _communicationBus = communicationBus;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetCities")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserSystemPolicyName)]
        [SwaggerOperation(
            Summary = "Gets cities",
            Description = "Gets cities",
            OperationId = "GetCities",
            Tags = new[] { "Cities" }
        )]
        [SwaggerResponse(200, "Cities collection.", typeof(CollectionResponse<CityResponse>))]
        public async Task<IActionResult> GetCitiesAsync([FromQuery]GetCitiesRequest request)
        {
            var getCitiesInputQuery = _mapper.Map<GetCitiesRequest, GetCitiesInputQuery>(request);
            var collectionOutputQuery = await _getCitiesQueryHandler.HandleAsync(getCitiesInputQuery);
            var collectionResponse = _mapper.Map<CollectionOutputQuery<CityOutputQuery>, CollectionResponse<CityResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetCity")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserSystemPolicyName)]
        [SwaggerOperation(
            Summary = "Gets a city",
            Description = "Gets a city",
            OperationId = "GetCity",
            Tags = new[] { "Cities" }
        )]
        [SwaggerResponse(200, "City.", typeof(CityResponse))]
        public async Task<IActionResult> GetCityAsync([FromRoute]Guid id)
        {
            var getCityOutputQuery = await _getCityQueryHandler.HandleAsync(new GetCityInputQuery(id));
            var cityResponse = _mapper.Map<CityOutputQuery, CityResponse>(getCityOutputQuery);
            return Ok(cityResponse);
        }

        [HttpPost("", Name = "CreateCity")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [SwaggerOperation(
            Summary = "Creates a new city",
            Description = "Creates a new city",
            OperationId = "CreateCity",
            Tags = new[] { "Cities" }
        )]
        [SwaggerResponse(201, "City created.", typeof(CityResponse))]
        [SwaggerResponse(409, "City already exists.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateCityAsync([FromBody]CreateCityRequest request)
        {
            var createCityCommand = _mapper.Map<CreateCityRequest, CreateCityCommand>(request);
            await _communicationBus.SendCommandAsync(createCityCommand);
            var getCityOutputQuery = await _getCityQueryHandler.HandleAsync(new GetCityInputQuery(createCityCommand.CityId));
            var cityResponse = _mapper.Map<CityOutputQuery, CityResponse>(getCityOutputQuery);

            return CreatedAtRoute("GetCity", new { id = cityResponse.Id }, cityResponse);
        }

        [HttpPut("{id}", Name = "UpdateCity")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Updates a city",
            Description = "Updates a city",
            OperationId = "UpdateCity",
            Tags = new[] { "Cities" }
        )]
        [SwaggerResponse(200, "City updated", typeof(CityResponse))]
        public async Task<IActionResult> UpdateCityAsync([FromRoute]Guid id, [FromBody]UpdateCityRequest request, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            ValidatePathIdWithRequestBodyId(id, request.Id);

            request.SetRowVersion(rowVersion);
            var updateCityCommand = _mapper.Map<UpdateCityRequest, UpdateCityCommand>(request);
            await _communicationBus.SendCommandAsync(updateCityCommand);
            var getCityOutputQuery = await _getCityQueryHandler.HandleAsync(new GetCityInputQuery(updateCityCommand.CityId));
            var cityResponse = _mapper.Map<CityOutputQuery, CityResponse>(getCityOutputQuery);

            return Ok(cityResponse);
        }

        [HttpDelete("{id}", Name = "DeleteCity")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Deletes a city",
            Description = "Deletes a city",
            OperationId = "DeleteCity",
            Tags = new[] { "Cities" }
        )]
        [SwaggerResponse(204, "City deleted.")]
        public async Task<IActionResult> DeleteCityAsync([FromRoute]Guid id, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            var deleteCityCommand = new DeleteCityCommand(id, rowVersion);
            await _communicationBus.SendCommandAsync(deleteCityCommand);

            return NoContent();
        }
    }
}