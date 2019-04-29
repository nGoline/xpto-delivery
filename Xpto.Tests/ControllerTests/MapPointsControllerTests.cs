using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xpto.Application.Controllers;
using Xpto.Domain.DTO;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service;
using Xpto.Tests.Common;
using Xpto.Tests.Fake;
using Xunit;

namespace Xpto.Tests.ControllerTests
{

    [TestCaseOrderer("Xpto.Tests.Common.PriorityOrderer", "Xpto.Tests")]
    public class MapPointsControllerTests
    {
        private Guid _mapPointId;
        private MapPointsController _controller;
        private IMapPointService _service;

        public MapPointsControllerTests()
        {
            _service = new FakeMapPointService();
            _controller = new MapPointsController(_service);
        }

        #region Add()
        [Fact, TestPriority(0)]
        public async Task Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new MapPoint(string.Empty, 15, -15);

            // Act
            var badResponse = await _controller.Create(new MapPointDTO(nameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }


        [Fact, TestPriority(1)]
        public async Task Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var testItem = new MapPoint("Test 4", -15, 15);

            // Act
            var createdResponse = await _controller.Create(new MapPointDTO(testItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);

            _mapPointId = testItem.Id;
        }


        [Fact, TestPriority(2)]
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

        #region Get()
        [Fact, TestPriority(3)]
        public async Task Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact, TestPriority(4)]
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
        [Fact, TestPriority(5)]
        public async Task GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact, TestPriority(6)]
        public async Task GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.Get(_mapPointId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact, TestPriority(7)]
        public async Task GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            // Act
            var okResult = (await _controller.Get(_mapPointId)).Result as OkObjectResult;

            // Assert
            Assert.IsType<MapPointDTO>(okResult.Value);
            Assert.Equal(_mapPointId, (okResult.Value as MapPointDTO).Id);
        }
        #endregion
    }
}