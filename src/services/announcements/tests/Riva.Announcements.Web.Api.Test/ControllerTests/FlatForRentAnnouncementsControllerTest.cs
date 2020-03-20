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
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
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
    public class FlatForRentAnnouncementsControllerTest
    {
        private readonly Mock<IQueryHandler<GetFlatForRentAnnouncementsInputQuery, CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>>> _getFlatForRentAnnouncementsQueryHandlerMock;
        private readonly Mock<IQueryHandler<GetFlatForRentAnnouncementInputQuery, FlatForRentAnnouncementOutputQuery>> _getFlatForRentAnnouncementQueryHandlerMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FlatForRentAnnouncementsController _controller;

        public FlatForRentAnnouncementsControllerTest()
        {
            _getFlatForRentAnnouncementsQueryHandlerMock = new Mock<IQueryHandler<GetFlatForRentAnnouncementsInputQuery, CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>>>();
            _getFlatForRentAnnouncementQueryHandlerMock = new Mock<IQueryHandler<GetFlatForRentAnnouncementInputQuery, FlatForRentAnnouncementOutputQuery>>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _mapperMock = new Mock<IMapper>();
            _controller = new FlatForRentAnnouncementsController(_getFlatForRentAnnouncementsQueryHandlerMock.Object,
                _getFlatForRentAnnouncementQueryHandlerMock.Object, _communicationBusMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetFlatForRentAnnouncementsAsync_Should_Return_OkObjectResult_With_CollectionResponse_With_FlatForRentAnnouncementResponses()
        {
            var flatForRentAnnouncementOutputQueries = new List<FlatForRentAnnouncementOutputQuery>
            {
                new FlatForRentAnnouncementOutputQuery(Guid.NewGuid(), "Title", "http://sourceUrl",
                    Guid.NewGuid(), DateTimeOffset.UtcNow, "Description", 100, NumberOfRoomsEnumeration.One, 
                    new List<Guid>())
            };
            var collectionOutputQuery =
                new CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>(flatForRentAnnouncementOutputQueries.Count, flatForRentAnnouncementOutputQueries);
            var flatForRentAnnouncementResponses = flatForRentAnnouncementOutputQueries.Select(output =>
                new FlatForRentAnnouncementResponse(output.Id, output.Title, output.SourceUrl, output.CityId,
                    output.Created, output.Description, output.Price,
                    FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnum(output.NumberOfRooms),
                    output.CityDistricts))
                .ToList();
            var collectionResponse = new CollectionResponse<FlatForRentAnnouncementResponse>(flatForRentAnnouncementResponses.Count, flatForRentAnnouncementResponses);

            _getFlatForRentAnnouncementsQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetFlatForRentAnnouncementsInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(collectionOutputQuery);
            _mapperMock
                .Setup(x => x.Map<CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>, CollectionResponse<FlatForRentAnnouncementResponse>>(It.IsAny<CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>>()))
                .Returns(collectionResponse);

            var result = await _controller.GetFlatForRentAnnouncementsAsync(new GetFlatForRentAnnouncementsRequest());
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(collectionResponse);
        }

        [Fact]
        public async Task GetFlatForRentAnnouncementAsync_Should_Return_OkObjectResult_With_FlatForRentAnnouncementResponses()
        {
            var flatForRentAnnouncementOutputQuery = new FlatForRentAnnouncementOutputQuery(Guid.NewGuid(), "Title",
                "http://sourceUrl", Guid.NewGuid(), DateTimeOffset.UtcNow, "Description", 100,
                NumberOfRoomsEnumeration.One, new List<Guid>());
            var flatForRentAnnouncementResponse = new FlatForRentAnnouncementResponse(
                flatForRentAnnouncementOutputQuery.Id, 
                flatForRentAnnouncementOutputQuery.Title,
                flatForRentAnnouncementOutputQuery.SourceUrl, 
                flatForRentAnnouncementOutputQuery.CityId,
                flatForRentAnnouncementOutputQuery.Created, 
                flatForRentAnnouncementOutputQuery.Description,
                flatForRentAnnouncementOutputQuery.Price,
                FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnum(flatForRentAnnouncementOutputQuery.NumberOfRooms), 
                flatForRentAnnouncementOutputQuery.CityDistricts);

            _getFlatForRentAnnouncementQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetFlatForRentAnnouncementInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(flatForRentAnnouncementOutputQuery);
            _mapperMock
                .Setup(x => x.Map<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>(It.IsAny<FlatForRentAnnouncementOutputQuery>()))
                .Returns(flatForRentAnnouncementResponse);

            var result = await _controller.GetFlatForRentAnnouncementAsync(flatForRentAnnouncementOutputQuery.Id);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(flatForRentAnnouncementResponse);
        }

        [Fact]
        public async Task CreateFlatForRentAnnouncementAsync_Should_Return_CreatedAtRouteResult_With_FlatForRentAnnouncementResponse()
        {
            var createFlatForRentAnnouncementRequest = new CreateFlatForRentAnnouncementRequest
            {
                Title = "Title",
                SourceUrl = "http://sourceUrl",
                CityId = Guid.NewGuid(),
                Description = "Description",
                Price = 100,
                NumberOfRooms = NumberOfRooms.One,
                CityDistricts = new List<Guid> { Guid.NewGuid() }
            };
            var createFlatForRentAnnouncementCommand = new CreateFlatForRentAnnouncementCommand(
                Guid.NewGuid(),
                createFlatForRentAnnouncementRequest.Title, 
                createFlatForRentAnnouncementRequest.SourceUrl,
                createFlatForRentAnnouncementRequest.CityId, 
                createFlatForRentAnnouncementRequest.Description,
                createFlatForRentAnnouncementRequest.Price,
                FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnumeration(createFlatForRentAnnouncementRequest.NumberOfRooms), 
                createFlatForRentAnnouncementRequest.CityDistricts);
            var flatForRentAnnouncementOutputQuery = new FlatForRentAnnouncementOutputQuery(
                createFlatForRentAnnouncementCommand.FlatForRentAnnouncementId, 
                createFlatForRentAnnouncementCommand.Title, 
                createFlatForRentAnnouncementCommand.SourceUrl, 
                createFlatForRentAnnouncementCommand.CityId, 
                DateTimeOffset.UtcNow, 
                createFlatForRentAnnouncementCommand.Description, 
                createFlatForRentAnnouncementCommand.Price, 
                createFlatForRentAnnouncementCommand.NumberOfRooms, 
                createFlatForRentAnnouncementCommand.CityDistricts);
            var flatForRentAnnouncementResponse = new FlatForRentAnnouncementResponse(
                flatForRentAnnouncementOutputQuery.Id, 
                flatForRentAnnouncementOutputQuery.Title, 
                flatForRentAnnouncementOutputQuery.SourceUrl, 
                flatForRentAnnouncementOutputQuery.CityId, 
                flatForRentAnnouncementOutputQuery.Created, 
                flatForRentAnnouncementOutputQuery.Description, 
                flatForRentAnnouncementOutputQuery.Price, 
                FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnum(flatForRentAnnouncementOutputQuery.NumberOfRooms), 
                flatForRentAnnouncementOutputQuery.CityDistricts);

            _mapperMock
                .Setup(x => x.Map<CreateFlatForRentAnnouncementRequest, CreateFlatForRentAnnouncementCommand>(It.IsAny<CreateFlatForRentAnnouncementRequest>()))
                .Returns(createFlatForRentAnnouncementCommand);
            _communicationBusMock
                .Setup(x => x.SendCommandAsync(It.IsAny<CreateFlatForRentAnnouncementCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getFlatForRentAnnouncementQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetFlatForRentAnnouncementInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(flatForRentAnnouncementOutputQuery);
            _mapperMock
                .Setup(x => x.Map<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>(It.IsAny<FlatForRentAnnouncementOutputQuery>()))
                .Returns(flatForRentAnnouncementResponse);

            var result = await _controller.CreateFlatForRentAnnouncementAsync(createFlatForRentAnnouncementRequest);
            var createdAtRouteResult = result.As<CreatedAtRouteResult>();

            createdAtRouteResult.Value.Should().BeEquivalentTo(flatForRentAnnouncementResponse);
            createdAtRouteResult.RouteName.Should().BeEquivalentTo("GetFlatForRentAnnouncement");
            createdAtRouteResult.RouteValues.Should()
                .BeEquivalentTo(new Microsoft.AspNetCore.Routing.RouteValueDictionary(new { id = flatForRentAnnouncementResponse.Id }));
        }

        [Fact]
        public async Task UpdateFlatForRentAnnouncementAsync_Should_Return_OkObjectResult_With_FlatForRentAnnouncementResponse()
        {
            var flatForRentAnnouncementId = Guid.NewGuid();
            var updateFlatForRentAnnouncementRequest = new UpdateFlatForRentAnnouncementRequest
            {
                Id = flatForRentAnnouncementId,
                Title = "NewTitle",
                SourceUrl = "http://sourceUrl",
                CityId = Guid.NewGuid(),
                Description = "Description",
                Price = 100,
                NumberOfRooms = NumberOfRooms.One,
                CityDistricts = new List<Guid> { Guid.NewGuid() }
            };
            var updateFlatForRentAnnouncementCommand = new UpdateFlatForRentAnnouncementCommand(
                flatForRentAnnouncementId,
                updateFlatForRentAnnouncementRequest.Title,
                updateFlatForRentAnnouncementRequest.SourceUrl,
                updateFlatForRentAnnouncementRequest.CityId,
                updateFlatForRentAnnouncementRequest.Description,
                updateFlatForRentAnnouncementRequest.Price,
                FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnumeration(updateFlatForRentAnnouncementRequest.NumberOfRooms),
                updateFlatForRentAnnouncementRequest.CityDistricts);
            var flatForRentAnnouncementOutputQuery = new FlatForRentAnnouncementOutputQuery(
                updateFlatForRentAnnouncementCommand.FlatForRentAnnouncementId,
                updateFlatForRentAnnouncementCommand.Title,
                updateFlatForRentAnnouncementCommand.SourceUrl,
                updateFlatForRentAnnouncementCommand.CityId,
                DateTimeOffset.UtcNow,
                updateFlatForRentAnnouncementCommand.Description,
                updateFlatForRentAnnouncementCommand.Price,
                updateFlatForRentAnnouncementCommand.NumberOfRooms,
                updateFlatForRentAnnouncementCommand.CityDistricts);
            var flatForRentAnnouncementResponse = new FlatForRentAnnouncementResponse(
                flatForRentAnnouncementOutputQuery.Id,
                flatForRentAnnouncementOutputQuery.Title,
                flatForRentAnnouncementOutputQuery.SourceUrl,
                flatForRentAnnouncementOutputQuery.CityId,
                flatForRentAnnouncementOutputQuery.Created,
                flatForRentAnnouncementOutputQuery.Description,
                flatForRentAnnouncementOutputQuery.Price,
                FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnum(flatForRentAnnouncementOutputQuery.NumberOfRooms),
                flatForRentAnnouncementOutputQuery.CityDistricts);

            _mapperMock
                .Setup(x => x.Map<UpdateFlatForRentAnnouncementRequest, UpdateFlatForRentAnnouncementCommand>(It.IsAny<UpdateFlatForRentAnnouncementRequest>()))
                .Returns(updateFlatForRentAnnouncementCommand);
            _communicationBusMock
                .Setup(x => x.SendCommandAsync(It.IsAny<UpdateFlatForRentAnnouncementCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getFlatForRentAnnouncementQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetFlatForRentAnnouncementInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(flatForRentAnnouncementOutputQuery);
            _mapperMock
                .Setup(x => x.Map<FlatForRentAnnouncementOutputQuery, FlatForRentAnnouncementResponse>(It.IsAny<FlatForRentAnnouncementOutputQuery>()))
                .Returns(flatForRentAnnouncementResponse);

            var result = await _controller.UpdateFlatForRentAnnouncementAsync(flatForRentAnnouncementId, updateFlatForRentAnnouncementRequest);
            var okObjectResult = result.As<OkObjectResult>();

            okObjectResult.Value.Should().BeEquivalentTo(flatForRentAnnouncementResponse);
        }

        [Fact]
        public async Task DeleteFlatForRentAnnouncementAsync_Should_Return_NoContentResult()
        {
            var flatForRentAnnouncementId = Guid.NewGuid();

            _communicationBusMock
                .Setup(x => x.SendCommandAsync(It.IsAny<DeleteFlatForRentAnnouncementCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteFlatForRentAnnouncementAsync(flatForRentAnnouncementId);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }
    }
}