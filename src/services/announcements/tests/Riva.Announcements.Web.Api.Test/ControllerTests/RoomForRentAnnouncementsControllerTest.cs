using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Web.Api.AutoMapperProfiles;
using Riva.Announcements.Web.Api.Controllers;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.Announcements.Web.Api.Models.Requests;
using Riva.Announcements.Web.Api.Models.Responses;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Xunit;

namespace Riva.Announcements.Web.Api.Test.ControllerTests
{
    public class RoomForRentAnnouncementsControllerTest
    {
        private readonly Mock<IQueryHandler<GetRoomForRentAnnouncementsInputQuery, CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>>> _getRoomForRentAnnouncementsQueryHandlerMock;
        private readonly Mock<IQueryHandler<GetRoomForRentAnnouncementInputQuery, RoomForRentAnnouncementOutputQuery>> _getRoomForRentAnnouncementQueryHandlerMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RoomForRentAnnouncementsController _controller;

        public RoomForRentAnnouncementsControllerTest()
        {
            _getRoomForRentAnnouncementsQueryHandlerMock = new Mock<IQueryHandler<GetRoomForRentAnnouncementsInputQuery, CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>>>();
            _getRoomForRentAnnouncementQueryHandlerMock = new Mock<IQueryHandler<GetRoomForRentAnnouncementInputQuery, RoomForRentAnnouncementOutputQuery>>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RoomForRentAnnouncementsController(_getRoomForRentAnnouncementsQueryHandlerMock.Object,
                _getRoomForRentAnnouncementQueryHandlerMock.Object, _communicationBusMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetRoomForRentAnnouncementsAsync_Should_Return_OkObjectResult_With_CollectionResponse_With_RoomForRentAnnouncementResponses()
        {
            var roomForRentAnnouncementOutputQueries = new List<RoomForRentAnnouncementOutputQuery>
            {
                new RoomForRentAnnouncementOutputQuery(Guid.NewGuid(), "Title", "http://sourceUrl", Guid.NewGuid(),
                    DateTimeOffset.UtcNow, "Description", 100, new List<RoomTypeEnumeration>(), new List<Guid>())
            };
            var collectionOutputQuery =
                new CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>(roomForRentAnnouncementOutputQueries.Count, roomForRentAnnouncementOutputQueries);
            var roomForRentAnnouncementResponses = roomForRentAnnouncementOutputQueries.Select(output =>
                new RoomForRentAnnouncementResponse(output.Id, output.Title, output.SourceUrl, output.CityId, output.Created, 
                    output.Description, output.Price, output.RoomTypes.Select(RoomForRentAnnouncementProfile.ConvertToRoomTypeEnum),
                    output.CityDistricts))
                .ToList();
            var collectionResponse = new CollectionResponse<RoomForRentAnnouncementResponse>(roomForRentAnnouncementResponses.Count, roomForRentAnnouncementResponses);

            _getRoomForRentAnnouncementsQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRoomForRentAnnouncementsInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(collectionOutputQuery);
            _mapperMock
                .Setup(x => x.Map<CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>, CollectionResponse<RoomForRentAnnouncementResponse>>(It.IsAny<CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>>()))
                .Returns(collectionResponse);

            var result = await _controller.GetRoomForRentAnnouncementsAsync(new GetRoomForRentAnnouncementsRequest());
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(collectionResponse);
        }

        [Fact]
        public async Task GetRoomForRentAnnouncementAsync_Should_Return_OkObjectResult_With_RoomForRentAnnouncementResponses()
        {
            var roomForRentAnnouncementOutputQuery = new RoomForRentAnnouncementOutputQuery(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), DateTimeOffset.UtcNow, "Description", 100,
                new List<RoomTypeEnumeration>(), new List<Guid>());
            var roomForRentAnnouncementResponse = new RoomForRentAnnouncementResponse(
                roomForRentAnnouncementOutputQuery.Id,
                roomForRentAnnouncementOutputQuery.Title,
                roomForRentAnnouncementOutputQuery.SourceUrl,
                roomForRentAnnouncementOutputQuery.CityId,
                roomForRentAnnouncementOutputQuery.Created,
                roomForRentAnnouncementOutputQuery.Description,
                roomForRentAnnouncementOutputQuery.Price,
                roomForRentAnnouncementOutputQuery.RoomTypes.Select(RoomForRentAnnouncementProfile.ConvertToRoomTypeEnum),
                roomForRentAnnouncementOutputQuery.CityDistricts);

            _getRoomForRentAnnouncementQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRoomForRentAnnouncementInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roomForRentAnnouncementOutputQuery);
            _mapperMock
                .Setup(x => x.Map<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>(It.IsAny<RoomForRentAnnouncementOutputQuery>()))
                .Returns(roomForRentAnnouncementResponse);

            var result = await _controller.GetRoomForRentAnnouncementAsync(roomForRentAnnouncementOutputQuery.Id);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(roomForRentAnnouncementResponse);
        }

        [Fact]
        public async Task CreateRoomForRentAnnouncementAsync_Should_Return_CreatedAtRouteResult_With_RoomForRentAnnouncementResponse()
        {
            var createRoomForRentAnnouncementRequest = new CreateRoomForRentAnnouncementRequest
            {
                Title = "Title",
                SourceUrl = "http://sourceUrl",
                CityId = Guid.NewGuid(),
                Description = "Description",
                Price = 100,
                RoomTypes = new List<RoomType>{ RoomType.Double },
                CityDistricts = new List<Guid> { Guid.NewGuid() }
            };
            var createRoomForRentAnnouncementCommand = new CreateRoomForRentAnnouncementCommand(
                Guid.NewGuid(),
                createRoomForRentAnnouncementRequest.Title,
                createRoomForRentAnnouncementRequest.SourceUrl,
                createRoomForRentAnnouncementRequest.CityId,
                createRoomForRentAnnouncementRequest.Description,
                createRoomForRentAnnouncementRequest.Price,
                createRoomForRentAnnouncementRequest.RoomTypes.Select(RoomForRentAnnouncementProfile.ConvertToRoomTypeEnumeration),
                createRoomForRentAnnouncementRequest.CityDistricts);
            var roomForRentAnnouncementOutputQuery = new RoomForRentAnnouncementOutputQuery(
                createRoomForRentAnnouncementCommand.RoomForRentAnnouncementId,
                createRoomForRentAnnouncementCommand.Title,
                createRoomForRentAnnouncementCommand.SourceUrl,
                createRoomForRentAnnouncementCommand.CityId,
                DateTimeOffset.UtcNow,
                createRoomForRentAnnouncementCommand.Description,
                createRoomForRentAnnouncementCommand.Price,
                createRoomForRentAnnouncementCommand.RoomTypes,
                createRoomForRentAnnouncementCommand.CityDistricts);
            var roomForRentAnnouncementResponse = new RoomForRentAnnouncementResponse(
                roomForRentAnnouncementOutputQuery.Id,
                roomForRentAnnouncementOutputQuery.Title,
                roomForRentAnnouncementOutputQuery.SourceUrl,
                roomForRentAnnouncementOutputQuery.CityId,
                roomForRentAnnouncementOutputQuery.Created,
                roomForRentAnnouncementOutputQuery.Description,
                roomForRentAnnouncementOutputQuery.Price,
                roomForRentAnnouncementOutputQuery.RoomTypes.Select(RoomForRentAnnouncementProfile.ConvertToRoomTypeEnum),
                roomForRentAnnouncementOutputQuery.CityDistricts);

            _mapperMock
                .Setup(x => x.Map<CreateRoomForRentAnnouncementRequest, CreateRoomForRentAnnouncementCommand>(It.IsAny<CreateRoomForRentAnnouncementRequest>()))
                .Returns(createRoomForRentAnnouncementCommand);
            _communicationBusMock
                .Setup(x => x.SendCommandAsync(It.IsAny<CreateRoomForRentAnnouncementCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getRoomForRentAnnouncementQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRoomForRentAnnouncementInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roomForRentAnnouncementOutputQuery);
            _mapperMock
                .Setup(x => x.Map<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>(It.IsAny<RoomForRentAnnouncementOutputQuery>()))
                .Returns(roomForRentAnnouncementResponse);

            var result = await _controller.CreateRoomForRentAnnouncementAsync(createRoomForRentAnnouncementRequest);
            var createdAtRouteResult = result.As<CreatedAtRouteResult>();

            createdAtRouteResult.Value.Should().BeEquivalentTo(roomForRentAnnouncementResponse);
            createdAtRouteResult.RouteName.Should().BeEquivalentTo("GetRoomForRentAnnouncement");
            createdAtRouteResult.RouteValues.Should()
                .BeEquivalentTo(new Microsoft.AspNetCore.Routing.RouteValueDictionary(new { id = roomForRentAnnouncementResponse.Id }));
        }

        [Fact]
        public async Task UpdateRoomForRentAnnouncementAsync_Should_Return_OkObjectResult_With_RoomForRentAnnouncementResponse()
        {
            var roomForRentAnnouncementId = Guid.NewGuid();
            var updateRoomForRentAnnouncementRequest = new UpdateRoomForRentAnnouncementRequest
            {
                Id = roomForRentAnnouncementId,
                Title = "NewTitle",
                SourceUrl = "http://sourceUrl",
                CityId = Guid.NewGuid(),
                Description = "Description",
                Price = 100,
                RoomTypes = new List<RoomType>{ RoomType.Single },
                CityDistricts = new List<Guid> { Guid.NewGuid() }
            };
            var updateRoomForRentAnnouncementCommand = new UpdateRoomForRentAnnouncementCommand(
                roomForRentAnnouncementId,
                updateRoomForRentAnnouncementRequest.Title,
                updateRoomForRentAnnouncementRequest.SourceUrl,
                updateRoomForRentAnnouncementRequest.CityId,
                updateRoomForRentAnnouncementRequest.Description,
                updateRoomForRentAnnouncementRequest.Price,
                updateRoomForRentAnnouncementRequest.RoomTypes.Select(RoomForRentAnnouncementProfile.ConvertToRoomTypeEnumeration),
                updateRoomForRentAnnouncementRequest.CityDistricts);
            var roomForRentAnnouncementOutputQuery = new RoomForRentAnnouncementOutputQuery(
                updateRoomForRentAnnouncementCommand.RoomForRentAnnouncementId,
                updateRoomForRentAnnouncementCommand.Title,
                updateRoomForRentAnnouncementCommand.SourceUrl,
                updateRoomForRentAnnouncementCommand.CityId,
                DateTimeOffset.UtcNow,
                updateRoomForRentAnnouncementCommand.Description,
                updateRoomForRentAnnouncementCommand.Price,
                updateRoomForRentAnnouncementCommand.RoomTypes,
                updateRoomForRentAnnouncementCommand.CityDistricts);
            var roomForRentAnnouncementResponse = new RoomForRentAnnouncementResponse(
                roomForRentAnnouncementOutputQuery.Id,
                roomForRentAnnouncementOutputQuery.Title,
                roomForRentAnnouncementOutputQuery.SourceUrl,
                roomForRentAnnouncementOutputQuery.CityId,
                roomForRentAnnouncementOutputQuery.Created,
                roomForRentAnnouncementOutputQuery.Description,
                roomForRentAnnouncementOutputQuery.Price,
                roomForRentAnnouncementOutputQuery.RoomTypes.Select(RoomForRentAnnouncementProfile.ConvertToRoomTypeEnum),
                roomForRentAnnouncementOutputQuery.CityDistricts);

            _mapperMock
                .Setup(x => x.Map<UpdateRoomForRentAnnouncementRequest, UpdateRoomForRentAnnouncementCommand>(It.IsAny<UpdateRoomForRentAnnouncementRequest>()))
                .Returns(updateRoomForRentAnnouncementCommand);
            _communicationBusMock
                .Setup(x => x.SendCommandAsync(It.IsAny<UpdateRoomForRentAnnouncementCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getRoomForRentAnnouncementQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRoomForRentAnnouncementInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roomForRentAnnouncementOutputQuery);
            _mapperMock
                .Setup(x => x.Map<RoomForRentAnnouncementOutputQuery, RoomForRentAnnouncementResponse>(It.IsAny<RoomForRentAnnouncementOutputQuery>()))
                .Returns(roomForRentAnnouncementResponse);

            var result = await _controller.UpdateRoomForRentAnnouncementAsync(roomForRentAnnouncementId, updateRoomForRentAnnouncementRequest);
            var okObjectResult = result.As<OkObjectResult>();

            okObjectResult.Value.Should().BeEquivalentTo(roomForRentAnnouncementResponse);
        }

        [Fact]
        public async Task DeleteRoomForRentAnnouncementAsync_Should_Return_NoContentResult()
        {
            var roomForRentAnnouncementId = Guid.NewGuid();

            _communicationBusMock
                .Setup(x => x.SendCommandAsync(It.IsAny<DeleteRoomForRentAnnouncementCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteRoomForRentAnnouncementAsync(roomForRentAnnouncementId);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }
    }
}