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
    public class MapPointsControllerTests
    {
        MapPointsController _controller;
        IMapPointService _service;

        public MapPointsControllerTests()
        {
            _service = new FakeMapPointService();
            _controller = new MapPointsController(_service);
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
            var items = Assert.IsType<List<MapPointDTO>>(okResult.Value);
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
            Assert.IsType<MapPointDTO>(okResult.Value);
            Assert.Equal(testGuid, (okResult.Value as MapPointDTO).Id);
        }
        #endregion

        #region Add()
        [Fact]
        public async Task Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new MapPoint(string.Empty, 15, -15);

            // Act
            var badResponse = await _controller.Create(new MapPointDTO(nameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }


        [Fact]
        public async Task Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var testItem = new MapPoint("Test 4", -15, 15);

            // Act
            var createdResponse = await _controller.Create(new MapPointDTO(testItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }


        [Fact]
        public async Task Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new MapPoint("Test 4", -15, 15);

            // Act
            var createdResponse = (await _controller.Create(new MapPointDTO(testItem))) as CreatedAtActionResult;
            var item = createdResponse.Value as MapPointDTO;

            // Assert
            Assert.IsType<MapPointDTO>(item);
            Assert.Equal("Test 4", item.Name);
        }
        #endregion
    }
}