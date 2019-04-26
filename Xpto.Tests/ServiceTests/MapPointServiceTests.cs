using System;
using System.Linq;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xpto.Tests.Common;
using Xunit;
using Xpto.Data.Repository.EntityFramework;
using Xpto.Data.Context.Interfaces;
using Xpto.Data.Context;
using Moq;
using Xpto.Domain.Interfaces.Repository;
using System.Collections.Generic;
using Xpto.Domain.Interfaces.Service;
using Xpto.Tests.Fake;

namespace Xpto.Tests.ServiceTests
{
    [TestCaseOrderer("Xpto.Tests.Common.PriorityOrderer", "Xpto.Tests")]
    public class MapPointServiceTests
    {
        private IMapPointRepository _repository;
        private IMapPointService _service;
        private Guid _mapPointId;

        public MapPointServiceTests()
        {
            _repository = new FakeMapPointRepository();
            
            _service = new MapPointService(_repository, new FakeRouteService());
        }

        [Fact, TestPriority(0)]
        public async Task ShouldCreateEntry()
        {
            var mapPoint = new MapPoint("Test Point 1", -43.45554434, -110.04886744);

            await _service.AddAsync(mapPoint);

            Assert.NotEqual(Guid.Empty, mapPoint.Id);

            var fetchedMapPoint = await _repository.GetByIdAsync(mapPoint.Id);
            Assert.Equal(mapPoint.Name, fetchedMapPoint.Name);
            Assert.Equal(mapPoint.Latitude, fetchedMapPoint.Latitude);
            Assert.Equal(mapPoint.Longitude, fetchedMapPoint.Longitude);

            _mapPointId = fetchedMapPoint.Id;
        }

        [Fact, TestPriority(1)]
        public async Task ShouldDeleteEntry()
        {
            await _service.DeleteByIdAsync(_mapPointId);

            var mapPoint = await _service.GetByIdAsync(_mapPointId);

            Assert.Null(mapPoint);
        }
    }
}