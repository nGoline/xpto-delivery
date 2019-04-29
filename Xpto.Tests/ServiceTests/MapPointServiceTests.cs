using System.Diagnostics;
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
        private static IMapPointRepository _repository;
        private static IMapPointService _service;
        private static Guid _mapPointId;

        public MapPointServiceTests()
        {
            if(_repository == null)
                _repository = new FakeMapPointRepository();   

            if(_service == null)         
                _service = new MapPointService(_repository);
        }

        [Fact, TestPriority(0)]
        public async Task ShouldCreateEntry()
        {
            var mapPoint = new MapPoint("Test Point 1", -43.45554434, -110.04886744);

            await _service.AddAsync(mapPoint);

            Assert.NotEqual(Guid.Empty, mapPoint.Id);

            _mapPointId = mapPoint.Id;

            var fetchedMapPoint = await _service.GetByIdAsync(mapPoint.Id);
            Assert.Equal(mapPoint.Id, fetchedMapPoint.Id);
            Assert.Equal(mapPoint.Name, fetchedMapPoint.Name);
            Assert.Equal(mapPoint.Latitude, fetchedMapPoint.Latitude);
            Assert.Equal(mapPoint.Longitude, fetchedMapPoint.Longitude);
        }

        [Fact, TestPriority(1)]
        public async Task ShouldDeleteEntry()
        {
            Assert.NotEmpty(await _service.GetAllAsync());
            Debug.WriteLine((await _service.GetAllAsync()).FirstOrDefault().Id);
            await _service.DeleteByIdAsync(_mapPointId);

            var mapPoint = await _service.GetByIdAsync(_mapPointId);

            Assert.Null(mapPoint);
        }
    }
}