using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xpto.Application.Controllers;
using Xpto.Domain.DTO;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service;
using Xpto.Tests.Fake;
using Xunit;

namespace Xpto.Tests.ControllerTests
{
    public class RoutesControllerTests
    {
        RoutesController _controller;
        IRouteService _service;

        public RoutesControllerTests()
        {
            _service = new FakeRouteService();
            _controller = new RoutesController(_service);
        }

        #region Get()
        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = (await _controller.Get()).Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<RouteDTO>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }
        #endregion

        #region Get(id)
        [Fact]
        public async Task GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResult = await _controller.Get(testGuid);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResult = (await _controller.Get(testGuid)).Result as OkObjectResult;

            // Assert
            Assert.IsType<RouteDTO>(okResult.Value);
            Assert.Equal(testGuid, (okResult.Value as RouteDTO).Id);
        }
        #endregion

        #region Add()
        [Fact]
        public async Task Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new Route(Guid.NewGuid(), Guid.Empty, 1, 1);

            // Act
            var badResponse = await _controller.Create(new RouteDTO(nameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }


        [Fact]
        public async Task Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var testItem = new Route(Guid.NewGuid(), Guid.NewGuid(), 1, 1);

            // Act
            var createdResponse = await _controller.Create(new RouteDTO(testItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }


        [Fact]
        public async Task Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var mapPoint1 = new MapPoint("Test 1", 90, 118) { Id = Guid.NewGuid() };
            var mapPoint2 = new MapPoint("Test 1", 90.55883441, 117.99995994) { Id = Guid.NewGuid() };
            var testItem = new Route(mapPoint1.Id, mapPoint2.Id, 1, 1) { From = mapPoint1, To = mapPoint2 };

            // Act
            var createdResponse = (await _controller.Create(new RouteDTO(testItem))) as CreatedAtActionResult;
            var item = createdResponse.Value as RouteDTO;

            // Assert
            Assert.IsType<RouteDTO>(item);
            Assert.Equal(144, item.Cost);
        }
        #endregion
    }
}