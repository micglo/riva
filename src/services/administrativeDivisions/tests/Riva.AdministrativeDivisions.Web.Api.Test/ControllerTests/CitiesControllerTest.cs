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
    public class CitiesControllerTest
    {
        private readonly Mock<IQueryHandler<GetCitiesInputQuery, CollectionOutputQuery<CityOutputQuery>>> _getCitiesQueryHandlerMock;
        private readonly Mock<IQueryHandler<GetCityInputQuery, CityOutputQuery>> _getCityQueryHandlerMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CitiesController _controller;

        public CitiesControllerTest()
        {
            _getCitiesQueryHandlerMock = new Mock<IQueryHandler<GetCitiesInputQuery, CollectionOutputQuery<CityOutputQuery>>>();
            _getCityQueryHandlerMock = new Mock<IQueryHandler<GetCityInputQuery, CityOutputQuery>>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CitiesController(_getCitiesQueryHandlerMock.Object, _getCityQueryHandlerMock.Object,
                _communicationBusMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetCitiesAsync_Should_Return_OkObjectResult_With_CollectionResponse_With_CityResponses()
        {
            var getCitiesRequest = new GetCitiesRequest
            {
                Page = 1,
                PageSize = 100,
                StateId = Guid.NewGuid(),
                Name = "Name",
                PolishName = "PolishName",
                Sort = "name:asc"
            };
            var getCitiesInputQuery = new GetCitiesInputQuery(getCitiesRequest.Page,
                getCitiesRequest.PageSize, getCitiesRequest.Sort, getCitiesRequest.StateId, getCitiesRequest.Name,
                getCitiesRequest.PolishName);
            var cityOutputQuery = new CityOutputQuery(Guid.NewGuid(), Array.Empty<byte>(),
                "Name", "PolishName", Guid.NewGuid());
            var cityOutputQueries = new Collection<CityOutputQuery> { cityOutputQuery };
            var collectionOutputQuery = new CollectionOutputQuery<CityOutputQuery>(cityOutputQueries.Count, cityOutputQueries);
            var cityResponses = cityOutputQueries.Select(x =>
                new CityResponse(x.Id, x.RowVersion, x.Name, x.PolishName, x.StateId));
            var collectionResponse = new CollectionResponse<CityResponse>(cityOutputQueries.Count, cityResponses);

            _mapperMock.Setup(x => x.Map<GetCitiesRequest, GetCitiesInputQuery>(It.IsAny<GetCitiesRequest>()))
                .Returns(getCitiesInputQuery);
            _getCitiesQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCitiesInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(collectionOutputQuery);
            _mapperMock
                .Setup(x => x.Map<CollectionOutputQuery<CityOutputQuery>, CollectionResponse<CityResponse>>(It.IsAny<CollectionOutputQuery<CityOutputQuery>>()))
                .Returns(collectionResponse);

            var result = await _controller.GetCitiesAsync(getCitiesRequest);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(collectionResponse);
        }

        [Fact]
        public async Task GetCityAsync_Should_Return_OkObjectResult_With_CityResponse()
        {
            var cityOutputQuery = new CityOutputQuery(Guid.NewGuid(), Array.Empty<byte>(), "Name", "PolishName",
                Guid.NewGuid());
            var cityResponse = new CityResponse(cityOutputQuery.Id, cityOutputQuery.RowVersion, cityOutputQuery.Name,
                cityOutputQuery.PolishName, cityOutputQuery.StateId);

            _getCityQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCityInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cityOutputQuery);
            _mapperMock.Setup(x => x.Map<CityOutputQuery, CityResponse>(It.IsAny<CityOutputQuery>())).Returns(cityResponse);

            var result = await _controller.GetCityAsync(cityOutputQuery.Id);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(cityResponse);
        }

        [Fact]
        public async Task CreateCityAsync_Should_Return_CreatedAtRouteResult_With_CityResponse()
        {
            var createCityRequest = new CreateCityRequest { Name = "Name", PolishName = "PolishName" };
            var createCityCommand = new CreateCityCommand(Guid.NewGuid(), createCityRequest.Name,
                createCityRequest.PolishName, Guid.NewGuid());
            var cityOutputQuery = new CityOutputQuery(createCityCommand.CityId, Array.Empty<byte>(), createCityCommand.Name,
                createCityCommand.PolishName, createCityCommand.StateId);
            var cityResponse = new CityResponse(cityOutputQuery.Id, cityOutputQuery.RowVersion, cityOutputQuery.Name,
                cityOutputQuery.PolishName, cityOutputQuery.StateId);

            _mapperMock.Setup(x => x.Map<CreateCityRequest, CreateCityCommand>(It.IsAny<CreateCityRequest>())).Returns(createCityCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<CreateCityCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getCityQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCityInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cityOutputQuery);
            _mapperMock.Setup(x => x.Map<CityOutputQuery, CityResponse>(It.IsAny<CityOutputQuery>())).Returns(cityResponse);

            var result = await _controller.CreateCityAsync(createCityRequest);
            var createdAtRouteResult = result.As<CreatedAtRouteResult>();

            createdAtRouteResult.Value.Should().BeEquivalentTo(cityResponse);
            createdAtRouteResult.RouteName.Should().BeEquivalentTo("GetCity");
            createdAtRouteResult.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary(new { id = cityResponse.Id }));
        }

        [Fact]
        public async Task UpdateCityAsync_Should_Return_OkObjectResult_With_CityResponse()
        {
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 };
            var cityId = Guid.NewGuid();
            var updateCityRequest = new UpdateCityRequest
            {
                Id = cityId,
                Name = "Name",
                PolishName = "PolishName",
                StateId = Guid.NewGuid()
            };
            var updateCityCommand = new UpdateCityCommand(updateCityRequest.Id, rowVersion, updateCityRequest.Name,
                updateCityRequest.PolishName, updateCityRequest.StateId);
            var cityOutputQuery = new CityOutputQuery(updateCityCommand.CityId, new byte[] { 1, 2, 4, 8, 16, 32, 128 },
                updateCityCommand.Name, updateCityCommand.PolishName, updateCityCommand.StateId);
            var cityResponse = new CityResponse(cityOutputQuery.Id, cityOutputQuery.RowVersion,
                cityOutputQuery.Name, cityOutputQuery.PolishName, cityOutputQuery.StateId);

            _mapperMock.Setup(x => x.Map<UpdateCityRequest, UpdateCityCommand>(It.IsAny<UpdateCityRequest>()))
                .Returns(updateCityCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<UpdateCityCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getCityQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCityInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cityOutputQuery);
            _mapperMock.Setup(x => x.Map<CityOutputQuery, CityResponse>(It.IsAny<CityOutputQuery>())).Returns(cityResponse);

            var result = await _controller.UpdateCityAsync(cityId, updateCityRequest, rowVersion);
            var okObjectResult = result.As<OkObjectResult>();

            okObjectResult.Value.Should().BeEquivalentTo(cityResponse);
        }

        [Fact]
        public async Task DeleteCityAsync_Should_Return_NoContentResult()
        {
            var cityId = Guid.NewGuid();
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 };

            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<DeleteCityCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteCityAsync(cityId, rowVersion);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }
    }
}