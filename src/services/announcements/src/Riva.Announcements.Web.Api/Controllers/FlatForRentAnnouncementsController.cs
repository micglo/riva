using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Web.Api.Models.Requests;
using Riva.Announcements.Web.Api.Models.Responses;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Attributes.Routes;
using Riva.BuildingBlocks.WebApi.Controllers;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Swashbuckle.AspNetCore.Annotations;

namespace Riva.Announcements.Web.Api.Controllers
{
    [ApiRoute("flatForRentAnnouncements")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorSystemPolicyName)]
    public class FlatForRentAnnouncementsController : RivaControllerBase
    {
        private readonly IQueryHandler<GetFlatForRentAnnouncementsInputQuery, CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>> _getFlatForRentAnnouncementsQueryHandler;
        private readonly IQueryHandler<GetFlatForRentAnnouncementInputQuery, FlatForRentAnnouncementOutputQuery> _getFlatForRentAnnouncementQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IMapper _mapper;

        public FlatForRentAnnouncementsController(
            IQueryHandler<GetFlatForRentAnnouncementsInputQuery, CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>> getFlatForRentAnnouncementsQueryHandler, 
            IQueryHandler<GetFlatForRentAnnouncementInputQuery, FlatForRentAnnouncementOutputQuery> getFlatForRentAnnouncementQueryHandler, 
            ICommunicationBus communicationBus, IMapper mapper)
        {
            _getFlatForRentAnnouncementsQueryHandler = getFlatForRentAnnouncementsQueryHandler;
            _getFlatForRentAnnouncementQueryHandler = getFlatForRentAnnouncementQueryHandler;
            _communicationBus = communicationBus;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetFlatForRentAnnouncements")]
        [SwaggerOperation(
            Summary = "Gets flat for rent announcements",
            Description = "Gets flat for rent announcements",
            OperationId = "GetFlatForRentAnnouncements",
            Tags = new[] { "FlatForRentAnnouncements" }
        )]
        [SwaggerResponse(200, "FlatForRentAnnouncements collection.", typeof(CollectionResponse<FlatForRentAnnouncementResponse>))]
        public async Task<IActionResult> GetFlatForRentAnnouncementsAsync([FromQuery]GetFlatForRentAnnouncementsRequest request)
        {
            var getFlatForRentAnnouncementsInputQuery = _mapper.Map<GetFlatForRentAnnouncementsRequest, GetFlatForRentAnnouncementsInputQuery>(request);
            var collectionOutputQuery = await _getFlatForRentAnnouncementsQueryHandler.HandleAsync(getFlatForRentAnnouncementsInputQuery);
            var collectionResponse = _mapper.Map<CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>, CollectionResponse<FlatForRentAnnouncementResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetFlatForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Gets a flat for rent announcement",
            Description = "Gets a flat for rent announcement",
            OperationId = "GetFlatForRentAnnouncement",
            Tags = new[] { "FlatForRentAnnouncements" }
        )]
        [SwaggerResponse(200, "FlatForRentAnnouncement.", typeof(FlatForRentAnnouncementResponse))]
        public async Task<IActionResult> GetFlatForRentAnnouncementAsync(Guid id)
        {
            var getFlatForRentAnnouncementOutputQuery = await _getFlatForRentAnnouncementQueryHandler.HandleAsync(new GetFlatForRentAnnouncementInputQuery(id));
            var getFlatForRentAnnouncementResponse = _mapper.Map<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>(getFlatForRentAnnouncementOutputQuery);
            return Ok(getFlatForRentAnnouncementResponse);
        }

        [HttpPost("", Name = "CreateFlatForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Creates a new flat for rent announcement",
            Description = "Creates a new flat for rent announcement",
            OperationId = "CreateFlatForRentAnnouncement",
            Tags = new[] { "FlatForRentAnnouncements" }
        )]
        [SwaggerResponse(201, "FlatForRentAnnouncement created.", typeof(FlatForRentAnnouncementResponse))]
        public async Task<IActionResult> CreateFlatForRentAnnouncementAsync([FromBody]CreateFlatForRentAnnouncementRequest request)
        {
            var createFlatForRentAnnouncementCommand = _mapper.Map<CreateFlatForRentAnnouncementRequest, CreateFlatForRentAnnouncementCommand>(request);
            await _communicationBus.SendCommandAsync(createFlatForRentAnnouncementCommand);
            var getFlatForRentAnnouncementOutputQuery = await _getFlatForRentAnnouncementQueryHandler.HandleAsync(
                new GetFlatForRentAnnouncementInputQuery(createFlatForRentAnnouncementCommand
                    .FlatForRentAnnouncementId));
            var getFlatForRentAnnouncementResponse = _mapper.Map<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>(getFlatForRentAnnouncementOutputQuery);

            return CreatedAtRoute("GetFlatForRentAnnouncement", new { id = getFlatForRentAnnouncementResponse.Id }, getFlatForRentAnnouncementResponse);
        }

        [HttpPut("{id}", Name = "UpdateFlatForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Updates a flat for rent announcement",
            Description = "Updates a flat for rent announcement",
            OperationId = "UpdateFlatForRentAnnouncement",
            Tags = new[] { "FlatForRentAnnouncements" }
        )]
        [SwaggerResponse(200, "FlatForRentAnnouncement updated.", typeof(FlatForRentAnnouncementResponse))]
        public async Task<IActionResult> UpdateFlatForRentAnnouncementAsync([FromRoute] Guid id, [FromBody] UpdateFlatForRentAnnouncementRequest request)
        {
            ValidatePathIdWithRequestBodyId(id, request.Id);
            var updateFlatForRentAnnouncementCommand = _mapper.Map<UpdateFlatForRentAnnouncementRequest, UpdateFlatForRentAnnouncementCommand>(request);
            await _communicationBus.SendCommandAsync(updateFlatForRentAnnouncementCommand);
            var getFlatForRentAnnouncementOutputQuery = await _getFlatForRentAnnouncementQueryHandler.HandleAsync(
                new GetFlatForRentAnnouncementInputQuery(updateFlatForRentAnnouncementCommand
                    .FlatForRentAnnouncementId));
            var getFlatForRentAnnouncementResponse = _mapper.Map<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>(getFlatForRentAnnouncementOutputQuery);

            return Ok(getFlatForRentAnnouncementResponse);
        }

        [HttpDelete("{id}", Name = "DeleteFlatForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Deletes a flat for rent announcement",
            Description = "Deletes a flat for rent announcement",
            OperationId = "DeleteFlatForRentAnnouncement",
            Tags = new[] { "FlatForRentAnnouncements" }
        )]
        [SwaggerResponse(204, "FlatForRentAnnouncement deleted.")]
        public async Task<IActionResult> DeleteFlatForRentAnnouncementAsync([FromRoute]Guid id)
        {
            var deleteFlatForRentAnnouncementCommand = new DeleteFlatForRentAnnouncementCommand(id);
            await _communicationBus.SendCommandAsync(deleteFlatForRentAnnouncementCommand);
            return NoContent();
        }
    }
}