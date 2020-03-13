using System;
using System.Collections.Generic;
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
    public class CityDistrictsControllerTest
    {
        private readonly Mock<IQueryHandler<GetCityDistrictsInputQuery, CollectionOutputQuery<CityDistrictOutputQuery>>> _getCityDistrictsQueryHandlerMock;
        private readonly Mock<IQueryHandler<GetCityDistrictInputQuery, CityDistrictOutputQuery>> _getCityDistrictQueryHandlerMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CityDistrictsController _controller;

        public CityDistrictsControllerTest()
        {
            _getCityDistrictsQueryHandlerMock = new Mock<IQueryHandler<GetCityDistrictsInputQuery, CollectionOutputQuery<CityDistrictOutputQuery>>>();
            _getCityDistrictQueryHandlerMock = new Mock<IQueryHandler<GetCityDistrictInputQuery, CityDistrictOutputQuery>>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CityDistrictsController(_getCityDistrictsQueryHandlerMock.Object, _getCityDistrictQueryHandlerMock.Object,
                _communicationBusMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetCityDistrictsAsync_Should_Return_OkObjectResult_With_CollectionResponse_With_CityDistrictResponses()
        {
            var getCityDistrictsRequest = new GetCityDistrictsRequest
            {
                Page = 1,
                PageSize = 100,
                CityId = Guid.NewGuid(),
                Name = "Name",
                PolishName = "PolishName",
                Sort = "name:asc"
            };
            var getCityDistrictsInputQuery = new GetCityDistrictsInputQuery(getCityDistrictsRequest.Page,
                getCityDistrictsRequest.PageSize, getCityDistrictsRequest.Sort, getCityDistrictsRequest.Name,
                getCityDistrictsRequest.PolishName, getCityDistrictsRequest.CityId, null, new List<Guid>());
            var cityDistrictOutputQuery = new CityDistrictOutputQuery(Guid.NewGuid(), Array.Empty<byte>(),
                "Name", "PolishName", Guid.NewGuid(), Guid.NewGuid(), new List<string> { "NameVariant" });
            var cityDistrictOutputQueries = new Collection<CityDistrictOutputQuery> { cityDistrictOutputQuery };
            var collectionOutputQuery = new CollectionOutputQuery<CityDistrictOutputQuery>(cityDistrictOutputQueries.Count, cityDistrictOutputQueries);
            var cityDistrictResponses = cityDistrictOutputQueries.Select(x =>
                new CityDistrictResponse(x.Id, x.RowVersion, x.Name, x.PolishName, x.CityId, x.ParentId, x.NameVariants));
            var collectionResponse = new CollectionResponse<CityDistrictResponse>(cityDistrictOutputQueries.Count, cityDistrictResponses);

            _mapperMock.Setup(x => x.Map<GetCityDistrictsRequest, GetCityDistrictsInputQuery>(It.IsAny<GetCityDistrictsRequest>()))
                .Returns(getCityDistrictsInputQuery);
            _getCityDistrictsQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCityDistrictsInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(collectionOutputQuery);
            _mapperMock
                .Setup(x =>
                    x.Map<CollectionOutputQuery<CityDistrictOutputQuery>, CollectionResponse<CityDistrictResponse>>(
                        It.IsAny<CollectionOutputQuery<CityDistrictOutputQuery>>()))
                .Returns(collectionResponse);

            var result = await _controller.GetCityDistrictsAsync(getCityDistrictsRequest);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(collectionResponse);
        }

        [Fact]
        public async Task GetCityDistrictAsync_Should_Return_OkObjectResult_With_CityDistrictResponse()
        {
            var cityDistrictOutputQuery = new CityDistrictOutputQuery(Guid.NewGuid(), Array.Empty<byte>(), "Name",
                "PolishName", Guid.NewGuid(), Guid.NewGuid(), new List<string> { "NameVariant" });
            var cityDistrictResponse = new CityDistrictResponse(cityDistrictOutputQuery.Id,
                cityDistrictOutputQuery.RowVersion,
                cityDistrictOutputQuery.Name,
                cityDistrictOutputQuery.PolishName,
                cityDistrictOutputQuery.CityId,
                cityDistrictOutputQuery.ParentId,
                cityDistrictOutputQuery.NameVariants);

            _getCityDistrictQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCityDistrictInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cityDistrictOutputQuery);
            _mapperMock.Setup(x => x.Map<CityDistrictOutputQuery, CityDistrictResponse>(It.IsAny<CityDistrictOutputQuery>())).Returns(cityDistrictResponse);

            var result = await _controller.GetCityDistrictAsync(cityDistrictOutputQuery.Id);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(cityDistrictResponse);
        }

        [Fact]
        public async Task CreateCityDistrictAsync_Should_Return_CreatedAtRouteResult_With_CityDistrictResponse()
        {
            var createCityDistrictRequest = new CreateCityDistrictRequest
            {
                Name = "Name",
                PolishName = "PolishName",
                CityId = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                NameVariants = new List<string> { "NameVariant" }
            };
            var createCityDistrictCommand = new CreateCityDistrictCommand(Guid.NewGuid(),
                createCityDistrictRequest.Name,
                createCityDistrictRequest.PolishName,
                createCityDistrictRequest.CityId,
                createCityDistrictRequest.ParentId,
                createCityDistrictRequest.NameVariants);
            var cityDistrictOutputQuery = new CityDistrictOutputQuery(createCityDistrictCommand.CityDistrictId,
                Array.Empty<byte>(),
                createCityDistrictCommand.Name,
                createCityDistrictCommand.PolishName,
                createCityDistrictCommand.CityId,
                createCityDistrictCommand.ParentId,
                createCityDistrictCommand.NameVariants);
            var cityDistrictResponse = new CityDistrictResponse(cityDistrictOutputQuery.Id,
                cityDistrictOutputQuery.RowVersion,
                cityDistrictOutputQuery.Name,
                cityDistrictOutputQuery.PolishName,
                cityDistrictOutputQuery.CityId,
                cityDistrictOutputQuery.ParentId,
                cityDistrictOutputQuery.NameVariants);

            _mapperMock.Setup(x =>
                    x.Map<CreateCityDistrictRequest, CreateCityDistrictCommand>(It.IsAny<CreateCityDistrictRequest>()))
                .Returns(createCityDistrictCommand);
            _communicationBusMock.Setup(x =>
                    x.SendCommandAsync(It.IsAny<CreateCityDistrictCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getCityDistrictQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCityDistrictInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cityDistrictOutputQuery);
            _mapperMock.Setup(x => x.Map<CityDistrictOutputQuery, CityDistrictResponse>(It.IsAny<CityDistrictOutputQuery>())).Returns(cityDistrictResponse);

            var result = await _controller.CreateCityDistrictAsync(createCityDistrictRequest);
            var createdAtRouteResult = result.As<CreatedAtRouteResult>();

            createdAtRouteResult.Value.Should().BeEquivalentTo(cityDistrictResponse);
            createdAtRouteResult.RouteName.Should().BeEquivalentTo("GetCityDistrict");
            createdAtRouteResult.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary(new { id = cityDistrictResponse.Id }));
        }

        [Fact]
        public async Task UpdateCityDistrictAsync_Should_Return_OkObjectResult_With_CityDistrictResponse()
        {
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 };
            var cityDistrictId = Guid.NewGuid();
            var updateCityDistrictRequest = new UpdateCityDistrictRequest
            {
                Id = cityDistrictId,
                Name = "Name",
                PolishName = "PolishName",
                CityId = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                NameVariants = new List<string> { "NameVariant" }
            };
            var updateCityDistrictCommand = new UpdateCityDistrictCommand(updateCityDistrictRequest.Id,
                rowVersion,
                updateCityDistrictRequest.Name,
                updateCityDistrictRequest.PolishName,
                updateCityDistrictRequest.CityId,
                updateCityDistrictRequest.ParentId,
                updateCityDistrictRequest.NameVariants);
            var cityDistrictOutputQuery = new CityDistrictOutputQuery(updateCityDistrictCommand.CityDistrictId,
                new byte[] { 1, 2, 4, 8, 16, 32, 128 },
                updateCityDistrictCommand.Name,
                updateCityDistrictCommand.PolishName,
                updateCityDistrictCommand.CityId,
                updateCityDistrictCommand.ParentId,
                updateCityDistrictCommand.NameVariants);
            var cityDistrictResponse = new CityDistrictResponse(cityDistrictOutputQuery.Id,
                cityDistrictOutputQuery.RowVersion,
                cityDistrictOutputQuery.Name,
                cityDistrictOutputQuery.PolishName,
                cityDistrictOutputQuery.CityId,
                cityDistrictOutputQuery.ParentId,
                cityDistrictOutputQuery.NameVariants);

            _mapperMock.Setup(x => x.Map<UpdateCityDistrictRequest, UpdateCityDistrictCommand>(It.IsAny<UpdateCityDistrictRequest>()))
                .Returns(updateCityDistrictCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<UpdateCityDistrictCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getCityDistrictQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetCityDistrictInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cityDistrictOutputQuery);
            _mapperMock.Setup(x => x.Map<CityDistrictOutputQuery, CityDistrictResponse>(It.IsAny<CityDistrictOutputQuery>())).Returns(cityDistrictResponse);

            var result = await _controller.UpdateCityDistrictAsync(cityDistrictId, updateCityDistrictRequest, rowVersion);
            var okObjectResult = result.As<OkObjectResult>();

            okObjectResult.Value.Should().BeEquivalentTo(cityDistrictResponse);
        }

        [Fact]
        public async Task DeleteCityDistrictAsync_Should_Return_NoContentResult()
        {
            var cityDistrictId = Guid.NewGuid();
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 };

            _communicationBusMock.Setup(x =>
                    x.SendCommandAsync(It.IsAny<DeleteCityDistrictCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteCityDistrictAsync(cityDistrictId, rowVersion);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }
    }
}