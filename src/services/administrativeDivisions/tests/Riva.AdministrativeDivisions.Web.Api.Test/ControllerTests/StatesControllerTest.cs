using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Web.Api.Controllers;
using Riva.AdministrativeDivisions.Web.Api.Models.Requests;
using Riva.AdministrativeDivisions.Web.Api.Models.Responses;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.ControllerTests
{
    public class StatesControllerTest
    {
        private readonly Mock<IQueryHandler<GetStatesInputQuery, CollectionOutputQuery<StateOutputQuery>>> _getStatesQueryHandlerMock;
        private readonly Mock<IQueryHandler<GetStateInputQuery, StateOutputQuery>> _getStateQueryHandlerMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly StatesController _controller;

        public StatesControllerTest()
        {
            _getStatesQueryHandlerMock = new Mock<IQueryHandler<GetStatesInputQuery, CollectionOutputQuery<StateOutputQuery>>>();
            _getStateQueryHandlerMock = new Mock<IQueryHandler<GetStateInputQuery, StateOutputQuery>>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _mapperMock = new Mock<IMapper>();
            _controller = new StatesController(_getStatesQueryHandlerMock.Object, _getStateQueryHandlerMock.Object,
                _communicationBusMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetStatesAsync_Should_Return_OkObjectResult_With_CollectionResponse_With_StateResponses()
        {
            var getStatesRequest = new GetStatesRequest
            {
                Page = 1,
                PageSize = 100,
                Name = "Name",
                PolishName = "PolishName",
                Sort = "name:asc"
            };
            var getStatesInputQuery = new GetStatesInputQuery(getStatesRequest.Page,
                getStatesRequest.PageSize, getStatesRequest.Sort, getStatesRequest.Name, getStatesRequest.PolishName);
            var stateOutputQuery = new StateOutputQuery(Guid.NewGuid(), Array.Empty<byte>(), "Name", "PolishName");
            var stateOutputQueries = new Collection<StateOutputQuery> { stateOutputQuery };
            var collectionOutputQuery = new CollectionOutputQuery<StateOutputQuery>(stateOutputQueries.Count, stateOutputQueries);
            var stateResponses = stateOutputQueries.Select(x => new StateResponse(x.Id, x.RowVersion, x.Name, x.PolishName));
            var collectionResponse = new CollectionResponse<StateResponse>(stateOutputQueries.Count, stateResponses);

            _mapperMock.Setup(x => x.Map<GetStatesRequest, GetStatesInputQuery>(It.IsAny<GetStatesRequest>()))
                .Returns(getStatesInputQuery);
            _getStatesQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetStatesInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(collectionOutputQuery);
            _mapperMock
                .Setup(x => x.Map<CollectionOutputQuery<StateOutputQuery>, CollectionResponse<StateResponse>>(It.IsAny<CollectionOutputQuery<StateOutputQuery>>()))
                .Returns(collectionResponse);

            var result = await _controller.GetStatesAsync(getStatesRequest);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(collectionResponse);
        }

        [Fact]
        public async Task GetStateAsync_Should_Return_OkObjectResult_With_StateResponse()
        {
            var stateOutputQuery = new StateOutputQuery(Guid.NewGuid(), Array.Empty<byte>(), "Name", "PolishName");
            var stateResponse = new StateResponse(stateOutputQuery.Id, stateOutputQuery.RowVersion, stateOutputQuery.Name, stateOutputQuery.PolishName);

            _getStateQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetStateInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(stateOutputQuery);
            _mapperMock.Setup(x => x.Map<StateOutputQuery, StateResponse>(It.IsAny<StateOutputQuery>())).Returns(stateResponse);

            var result = await _controller.GetStateAsync(stateOutputQuery.Id);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(stateResponse);
        }

        [Fact]
        public async Task CreateStateAsync_Should_Return_CreatedAtRouteResult_With_StateResponse()
        {
            var createStateRequest = new CreateStateRequest { Name = "Name", PolishName = "PolishName" };
            var createStateCommand = new CreateStateCommand(Guid.NewGuid(), createStateRequest.Name, createStateRequest.PolishName);
            var stateOutputQuery = new StateOutputQuery(createStateCommand.StateId, Array.Empty<byte>(), createStateCommand.Name,
                createStateCommand.PolishName);
            var stateResponse = new StateResponse(stateOutputQuery.Id, stateOutputQuery.RowVersion, stateOutputQuery.Name, stateOutputQuery.PolishName);

            _mapperMock.Setup(x => x.Map<CreateStateRequest, CreateStateCommand>(It.IsAny<CreateStateRequest>())).Returns(createStateCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<CreateStateCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getStateQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetStateInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(stateOutputQuery);
            _mapperMock.Setup(x => x.Map<StateOutputQuery, StateResponse>(It.IsAny<StateOutputQuery>())).Returns(stateResponse);

            var result = await _controller.CreateStateAsync(createStateRequest);
            var createdAtRouteResult = result.As<CreatedAtRouteResult>();

            createdAtRouteResult.Value.Should().BeEquivalentTo(stateResponse);
            createdAtRouteResult.RouteName.Should().BeEquivalentTo("GetState");
            createdAtRouteResult.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary(new { id = stateResponse.Id }));
        }

        [Fact]
        public async Task UpdateStateAsync_Should_Return_OkObjectResult_With_StateResponse()
        {
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 };
            var stateId = Guid.NewGuid();
            var updateStateRequest = new UpdateStateRequest
            {
                Id = stateId,
                Name = "Name",
                PolishName = "PolishName"
            };
            var updateStateCommand = new UpdateStateCommand(updateStateRequest.Id, rowVersion, updateStateRequest.Name,
                updateStateRequest.PolishName);
            var stateOutputQuery = new StateOutputQuery(updateStateRequest.Id, new byte[] { 1, 2, 4, 8, 16, 32, 128 }, 
                updateStateRequest.Name, updateStateRequest.PolishName);
            var stateResponse = new StateResponse(stateOutputQuery.Id, stateOutputQuery.RowVersion, stateOutputQuery.Name, stateOutputQuery.PolishName);

            _mapperMock.Setup(x => x.Map<UpdateStateRequest, UpdateStateCommand>(It.IsAny<UpdateStateRequest>()))
                .Returns(updateStateCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<UpdateStateCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getStateQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetStateInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(stateOutputQuery);
            _mapperMock.Setup(x => x.Map<StateOutputQuery, StateResponse>(It.IsAny<StateOutputQuery>())).Returns(stateResponse);

            var result = await _controller.UpdateStateAsync(stateId, updateStateRequest, rowVersion);
            var okObjectResult = result.As<OkObjectResult>();

            okObjectResult.Value.Should().BeEquivalentTo(stateResponse);
        }

        [Fact]
        public async Task DeleteStateAsync_Should_Return_NoContentResult()
        {
            var stateId = Guid.NewGuid();
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 };

            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<DeleteStateCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteStateAsync(stateId, rowVersion);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }
    }
}