using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Queries;
using Riva.Identity.Web.Api.Controllers;
using Riva.Identity.Web.Api.Models.Requests.Roles;
using Riva.Identity.Web.Api.Models.Responses.Roles;
using Xunit;

namespace Riva.Identity.Web.Api.Test.ControllerTests
{
    public class RolesControllerTests
    {
        private readonly Mock<IQueryHandler<GetRolesInputQuery, CollectionOutputQuery<RoleOutputQuery>>> _getRolesQueryHandlerMock;
        private readonly Mock<IQueryHandler<GetRoleInputQuery, RoleOutputQuery>> _getRoleQueryHandlerMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RolesController _controller;

        public RolesControllerTests()
        {
            _getRolesQueryHandlerMock = new Mock<IQueryHandler<GetRolesInputQuery, CollectionOutputQuery<RoleOutputQuery>>>();
            _getRoleQueryHandlerMock = new Mock<IQueryHandler<GetRoleInputQuery, RoleOutputQuery>>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RolesController(_getRolesQueryHandlerMock.Object, _getRoleQueryHandlerMock.Object,
                _communicationBusMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetRolesAsync_Should_Return_OkObjectResult_With_CollectionResponse_With_GetRoleResponse()
        {
            var roleOutputQueries = new List<RoleOutputQuery>
            {
                new RoleOutputQuery(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.Administrator.DisplayName)
            };
            var collectionOutputQuery = new CollectionOutputQuery<RoleOutputQuery>(roleOutputQueries.Count, roleOutputQueries);
            var getRoleResponses = roleOutputQueries.Select(x => new GetRoleResponse(x.Id, x.RowVersion, x.Name));
            var collectionResponse = new CollectionResponse<GetRoleResponse>(roleOutputQueries.Count, getRoleResponses);

            _getRolesQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRolesInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(collectionOutputQuery);
            _mapperMock
                .Setup(x => x.Map<CollectionOutputQuery<RoleOutputQuery>, CollectionResponse<GetRoleResponse>>(It.IsAny<CollectionOutputQuery<RoleOutputQuery>>()))
                .Returns(collectionResponse);

            var result = await _controller.GetRolesAsync();
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(collectionResponse);
        }

        [Fact]
        public async Task GetRoleAsync_Should_Return_OkObjectResult_With_GetRoleResponse()
        {
            var roleOutputQuery = new RoleOutputQuery(Guid.NewGuid(), Array.Empty<byte>(),
                DefaultRoleEnumeration.Administrator.DisplayName);
            var getRoleResponse = new GetRoleResponse(roleOutputQuery.Id, roleOutputQuery.RowVersion, roleOutputQuery.Name);

            _getRoleQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRoleInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roleOutputQuery);
            _mapperMock.Setup(x => x.Map<RoleOutputQuery, GetRoleResponse>(It.IsAny<RoleOutputQuery>())).Returns(getRoleResponse);

            var result = await _controller.GetRoleAsync(roleOutputQuery.Id);
            var okResult = result.As<OkObjectResult>();

            okResult.Value.Should().BeEquivalentTo(getRoleResponse);
        }

        [Fact]
        public async Task CreateRoleAsync_Should_Return_CreatedAtRouteResult_With_GetRoleResponse()
        {
            var createRoleRequest = new CreateRoleRequest { Name = DefaultRoleEnumeration.Administrator.DisplayName };
            var createRoleCommand = new CreateRoleCommand(Guid.NewGuid(), createRoleRequest.Name);
            var roleOutputQuery = new RoleOutputQuery(createRoleCommand.RoleId, Array.Empty<byte>(), createRoleCommand.Name);
            var getRoleResponse = new GetRoleResponse(roleOutputQuery.Id, roleOutputQuery.RowVersion, roleOutputQuery.Name);

            _mapperMock.Setup(x => x.Map<CreateRoleRequest, CreateRoleCommand>(It.IsAny<CreateRoleRequest>())).Returns(createRoleCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<CreateRoleCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getRoleQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRoleInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roleOutputQuery);
            _mapperMock.Setup(x => x.Map<RoleOutputQuery, GetRoleResponse>(It.IsAny<RoleOutputQuery>())).Returns(getRoleResponse);

            var result = await _controller.CreateRoleAsync(createRoleRequest);
            var createdAtRouteResult = result.As<CreatedAtRouteResult>();

            createdAtRouteResult.Value.Should().BeEquivalentTo(getRoleResponse);
            createdAtRouteResult.RouteName.Should().BeEquivalentTo("GetRole");
            createdAtRouteResult.RouteValues.Should().BeEquivalentTo(new Microsoft.AspNetCore.Routing.RouteValueDictionary(new { id = roleOutputQuery.Id }));
        }

        [Fact]
        public async Task UpdateRoleAsync_Should_Return_OkObjectResult_With_GetRoleResponse()
        {
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32 };
            var roleId = Guid.NewGuid();
            var updateRoleRequest = new UpdateRoleRequest
            {
                Id = roleId,
                Name = DefaultRoleEnumeration.Administrator.DisplayName
            };
            var updateRoleCommand = new UpdateRoleCommand(updateRoleRequest.Id, rowVersion, updateRoleRequest.Name);
            var roleOutputQuery = new RoleOutputQuery(updateRoleRequest.Id, new byte[] { 1, 2, 4, 8, 16, 64 }, updateRoleRequest.Name);
            var getRoleResponse = new GetRoleResponse(roleOutputQuery.Id, roleOutputQuery.RowVersion, roleOutputQuery.Name);

            _mapperMock.Setup(x => x.Map<UpdateRoleRequest, UpdateRoleCommand>(It.IsAny<UpdateRoleRequest>())).Returns(updateRoleCommand);
            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<UpdateRoleCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _getRoleQueryHandlerMock
                .Setup(x => x.HandleAsync(It.IsAny<GetRoleInputQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(roleOutputQuery);
            _mapperMock.Setup(x => x.Map<RoleOutputQuery, GetRoleResponse>(It.IsAny<RoleOutputQuery>())).Returns(getRoleResponse);

            var result = await _controller.UpdateRoleAsync(roleId, updateRoleRequest, rowVersion);
            var okObjectResult = result.As<OkObjectResult>();

            okObjectResult.Value.Should().BeEquivalentTo(getRoleResponse);
        }

        [Fact]
        public async Task DeleteRoleAsync_Should_Return_NoContentResult()
        {
            var roleId = Guid.NewGuid();
            var rowVersion = new byte[] { 1, 2, 4, 8, 16, 32 };

            _communicationBusMock.Setup(x => x.SendCommandAsync(It.IsAny<DeleteRoleCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteRoleAsync(roleId, rowVersion);
            var noContentResult = result.As<NoContentResult>();

            noContentResult.Should().NotBeNull();
        }
    }
}