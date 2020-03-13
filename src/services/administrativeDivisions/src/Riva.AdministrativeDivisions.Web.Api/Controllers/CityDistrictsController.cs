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
    [ApiRoute("cityDistricts")]
    [ApiVersion("1")]
    [ApiController]
    public class CityDistrictsController : RivaControllerBase
    {
        private readonly IQueryHandler<GetCityDistrictsInputQuery, CollectionOutputQuery<CityDistrictOutputQuery>> _getCityDistrictsQueryHandler;
        private readonly IQueryHandler<GetCityDistrictInputQuery, CityDistrictOutputQuery> _getCityDistrictQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IMapper _mapper;

        public CityDistrictsController(IQueryHandler<GetCityDistrictsInputQuery, CollectionOutputQuery<CityDistrictOutputQuery>> getCityDistrictsQueryHandler, 
            IQueryHandler<GetCityDistrictInputQuery, CityDistrictOutputQuery> getCityDistrictQueryHandler, ICommunicationBus communicationBus, IMapper mapper)
        {
            _getCityDistrictsQueryHandler = getCityDistrictsQueryHandler;
            _getCityDistrictQueryHandler = getCityDistrictQueryHandler;
            _communicationBus = communicationBus;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetCityDistricts")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserSystemPolicyName)]
        [SwaggerOperation(
            Summary = "Gets cityDistricts",
            Description = "Gets cityDistricts",
            OperationId = "GetCityDistricts",
            Tags = new[] { "CityDistricts" }
        )]
        [SwaggerResponse(200, "CityDistricts collection.", typeof(CollectionResponse<CityDistrictResponse>))]
        public async Task<IActionResult> GetCityDistrictsAsync([FromQuery]GetCityDistrictsRequest request)
        {
            var getCityDistrictsInputQuery = _mapper.Map<GetCityDistrictsRequest, GetCityDistrictsInputQuery>(request);
            var collectionOutputQuery = await _getCityDistrictsQueryHandler.HandleAsync(getCityDistrictsInputQuery);
            var collectionResponse = _mapper.Map<CollectionOutputQuery<CityDistrictOutputQuery>, CollectionResponse<CityDistrictResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetCityDistrict")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerUserSystemPolicyName)]
        [SwaggerOperation(
            Summary = "Gets a cityDistrict",
            Description = "Gets a cityDistrict",
            OperationId = "GetCityDistrict",
            Tags = new[] { "CityDistricts" }
        )]
        [SwaggerResponse(200, "CityDistrict.", typeof(CityDistrictResponse))]
        public async Task<IActionResult> GetCityDistrictAsync([FromRoute]Guid id)
        {
            var getCityDistrictOutputQuery = await _getCityDistrictQueryHandler.HandleAsync(new GetCityDistrictInputQuery(id));
            var cityDistrictResponse = _mapper.Map<CityDistrictOutputQuery, CityDistrictResponse>(getCityDistrictOutputQuery);
            return Ok(cityDistrictResponse);
        }

        [HttpPost("", Name = "CreateCityDistrict")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [SwaggerOperation(
            Summary = "Creates a new cityDistrict",
            Description = "Creates a new cityDistrict",
            OperationId = "CreateCityDistrict",
            Tags = new[] { "CityDistricts" }
        )]
        [SwaggerResponse(201, "CityDistrict created.", typeof(CityDistrictResponse))]
        [SwaggerResponse(409, "CityDistrict already exists.", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateCityDistrictAsync([FromBody]CreateCityDistrictRequest request)
        {
            var createCityDistrictCommand = _mapper.Map<CreateCityDistrictRequest, CreateCityDistrictCommand>(request);
            await _communicationBus.SendCommandAsync(createCityDistrictCommand);
            var getCityDistrictOutputQuery = await _getCityDistrictQueryHandler.HandleAsync(new GetCityDistrictInputQuery(createCityDistrictCommand.CityDistrictId));
            var cityDistrictResponse = _mapper.Map<CityDistrictOutputQuery, CityDistrictResponse>(getCityDistrictOutputQuery);

            return CreatedAtRoute("GetCityDistrict", new { id = cityDistrictResponse.Id }, cityDistrictResponse);
        }

        [HttpPut("{id}", Name = "UpdateCityDistrict")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Updates a cityDistrict",
            Description = "Updates a cityDistrict",
            OperationId = "UpdateCityDistrict",
            Tags = new[] { "CityDistricts" }
        )]
        [SwaggerResponse(200, "CityDistrict updated", typeof(CityDistrictResponse))]
        public async Task<IActionResult> UpdateCityDistrictAsync([FromRoute]Guid id, [FromBody]UpdateCityDistrictRequest request, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            ValidatePathIdWithRequestBodyId(id, request.Id);

            request.SetRowVersion(rowVersion);
            var updateCityDistrictCommand = _mapper.Map<UpdateCityDistrictRequest, UpdateCityDistrictCommand>(request);
            await _communicationBus.SendCommandAsync(updateCityDistrictCommand);
            var getCityDistrictOutputQuery = await _getCityDistrictQueryHandler.HandleAsync(new GetCityDistrictInputQuery(updateCityDistrictCommand.CityDistrictId));
            var cityDistrictResponse = _mapper.Map<CityDistrictOutputQuery, CityDistrictResponse>(getCityDistrictOutputQuery);

            return Ok(cityDistrictResponse);
        }

        [HttpDelete("{id}", Name = "DeleteCityDistrict")]
        [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorPolicyName)]
        [RequireIfMatch]
        [SwaggerOperation(
            Summary = "Deletes a cityDistrict",
            Description = "Deletes a cityDistrict",
            OperationId = "DeleteCityDistrict",
            Tags = new[] { "CityDistricts" }
        )]
        [SwaggerResponse(204, "CityDistrict deleted.")]
        public async Task<IActionResult> DeleteCityDistrictAsync([FromRoute]Guid id, [FromHeader(Name = "If-Match")] byte[] rowVersion)
        {
            var deleteCityDistrictCommand = new DeleteCityDistrictCommand(id, rowVersion);
            await _communicationBus.SendCommandAsync(deleteCityDistrictCommand);

            return NoContent();
        }
    }
}