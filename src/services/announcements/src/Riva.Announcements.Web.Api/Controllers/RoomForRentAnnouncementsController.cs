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
    [ApiRoute("roomForRentAnnouncements")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(Policy = AuthorizationExtension.JwtBearerAdministratorSystemPolicyName)]
    public class RoomForRentAnnouncementsController : RivaControllerBase
    {
        private readonly IQueryHandler<GetRoomForRentAnnouncementsInputQuery, CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>> _getRoomForRentAnnouncementsQueryHandler;
        private readonly IQueryHandler<GetRoomForRentAnnouncementInputQuery, RoomForRentAnnouncementOutputQuery> _getRoomForRentAnnouncementQueryHandler;
        private readonly ICommunicationBus _communicationBus;
        private readonly IMapper _mapper;

        public RoomForRentAnnouncementsController(
            IQueryHandler<GetRoomForRentAnnouncementsInputQuery, CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>> getRoomForRentAnnouncementsQueryHandler,
            IQueryHandler<GetRoomForRentAnnouncementInputQuery, RoomForRentAnnouncementOutputQuery> getRoomForRentAnnouncementQueryHandler,
            ICommunicationBus communicationBus, IMapper mapper)
        {
            _getRoomForRentAnnouncementsQueryHandler = getRoomForRentAnnouncementsQueryHandler;
            _getRoomForRentAnnouncementQueryHandler = getRoomForRentAnnouncementQueryHandler;
            _communicationBus = communicationBus;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetRoomForRentAnnouncements")]
        [SwaggerOperation(
            Summary = "Gets flat for rent announcements",
            Description = "Gets flat for rent announcements",
            OperationId = "GetRoomForRentAnnouncements",
            Tags = new[] { "RoomForRentAnnouncements" }
        )]
        [SwaggerResponse(200, "RoomForRentAnnouncements collection.", typeof(CollectionResponse<RoomForRentAnnouncementResponse>))]
        public async Task<IActionResult> GetRoomForRentAnnouncementsAsync([FromQuery]GetRoomForRentAnnouncementsRequest request)
        {
            var getRoomForRentAnnouncementsInputQuery = _mapper.Map<GetRoomForRentAnnouncementsRequest, GetRoomForRentAnnouncementsInputQuery>(request);
            var collectionOutputQuery = await _getRoomForRentAnnouncementsQueryHandler.HandleAsync(getRoomForRentAnnouncementsInputQuery);
            var collectionResponse = _mapper.Map<CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>, CollectionResponse<RoomForRentAnnouncementResponse>>(collectionOutputQuery);
            return Ok(collectionResponse);
        }

        [HttpGet("{id}", Name = "GetRoomForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Gets a flat for rent announcement",
            Description = "Gets a flat for rent announcement",
            OperationId = "GetRoomForRentAnnouncement",
            Tags = new[] { "RoomForRentAnnouncements" }
        )]
        [SwaggerResponse(200, "RoomForRentAnnouncement.", typeof(RoomForRentAnnouncementResponse))]
        public async Task<IActionResult> GetRoomForRentAnnouncementAsync(Guid id)
        {
            var getRoomForRentAnnouncementOutputQuery = await _getRoomForRentAnnouncementQueryHandler.HandleAsync(new GetRoomForRentAnnouncementInputQuery(id));
            var getRoomForRentAnnouncementResponse = _mapper.Map<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>(getRoomForRentAnnouncementOutputQuery);
            return Ok(getRoomForRentAnnouncementResponse);
        }

        [HttpPost("", Name = "CreateRoomForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Creates a new flat for rent announcement",
            Description = "Creates a new flat for rent announcement",
            OperationId = "CreateRoomForRentAnnouncement",
            Tags = new[] { "RoomForRentAnnouncements" }
        )]
        [SwaggerResponse(201, "RoomForRentAnnouncement created.", typeof(RoomForRentAnnouncementResponse))]
        public async Task<IActionResult> CreateRoomForRentAnnouncementAsync([FromBody]CreateRoomForRentAnnouncementRequest request)
        {
            var createRoomForRentAnnouncementCommand = _mapper.Map<CreateRoomForRentAnnouncementRequest, CreateRoomForRentAnnouncementCommand>(request);
            await _communicationBus.SendCommandAsync(createRoomForRentAnnouncementCommand);
            var getRoomForRentAnnouncementOutputQuery = await _getRoomForRentAnnouncementQueryHandler.HandleAsync(
                new GetRoomForRentAnnouncementInputQuery(createRoomForRentAnnouncementCommand
                    .RoomForRentAnnouncementId));
            var getRoomForRentAnnouncementResponse = _mapper.Map<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>(getRoomForRentAnnouncementOutputQuery);

            return CreatedAtRoute("GetRoomForRentAnnouncement", new { id = getRoomForRentAnnouncementResponse.Id }, getRoomForRentAnnouncementResponse);
        }

        [HttpPut("{id}", Name = "UpdateRoomForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Updates a flat for rent announcement",
            Description = "Updates a flat for rent announcement",
            OperationId = "UpdateRoomForRentAnnouncement",
            Tags = new[] { "RoomForRentAnnouncements" }
        )]
        [SwaggerResponse(200, "RoomForRentAnnouncement updated.", typeof(RoomForRentAnnouncementResponse))]
        public async Task<IActionResult> UpdateRoomForRentAnnouncementAsync([FromRoute] Guid id, [FromBody] UpdateRoomForRentAnnouncementRequest request)
        {
            ValidatePathIdWithRequestBodyId(id, request.Id);
            var updateRoomForRentAnnouncementCommand = _mapper.Map<UpdateRoomForRentAnnouncementRequest, UpdateRoomForRentAnnouncementCommand>(request);
            await _communicationBus.SendCommandAsync(updateRoomForRentAnnouncementCommand);
            var getRoomForRentAnnouncementOutputQuery = await _getRoomForRentAnnouncementQueryHandler.HandleAsync(
                new GetRoomForRentAnnouncementInputQuery(updateRoomForRentAnnouncementCommand
                    .RoomForRentAnnouncementId));
            var getRoomForRentAnnouncementResponse = _mapper.Map<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>(getRoomForRentAnnouncementOutputQuery);

            return Ok(getRoomForRentAnnouncementResponse);
        }

        [HttpDelete("{id}", Name = "DeleteRoomForRentAnnouncement")]
        [SwaggerOperation(
            Summary = "Deletes a flat for rent announcement",
            Description = "Deletes a flat for rent announcement",
            OperationId = "DeleteRoomForRentAnnouncement",
            Tags = new[] { "RoomForRentAnnouncements" }
        )]
        [SwaggerResponse(204, "RoomForRentAnnouncement deleted.")]
        public async Task<IActionResult> DeleteRoomForRentAnnouncementAsync([FromRoute]Guid id)
        {
            var deleteRoomForRentAnnouncementCommand = new DeleteRoomForRentAnnouncementCommand(id);
            await _communicationBus.SendCommandAsync(deleteRoomForRentAnnouncementCommand);
            return NoContent();
        }
    }
}