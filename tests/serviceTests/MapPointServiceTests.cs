using System;
using System.Linq;
using System.Threading.Tasks;
using domain.contexts;
using domain.entities;
using domain.repositories;
using domain.services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using tests.helpers;
using Xunit;

namespace tests.serviceTests
{
    
    [TestCaseOrderer("tests.helpers.PriorityOrderer", "tests")]
    public class MapPointServiceTests : IDisposable
    {
        private readonly MainContextFactory _contextFactory;
        
        private Guid _mapPointId;

        public MapPointServiceTests()
        {
            _contextFactory = new MainContextFactory();
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                var mapPoints = await mapService.GetAllAsync();

                Assert.Empty(mapPoints);
            }
        }

        [Fact, TestPriority(1)]
        public async Task ShouldCreateEntry()
        {
            var mapPoint = new MapPointEntity("Test Point 1", -43.45554434, -110.04886744);
            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                await mapService.AddAsync(mapPoint);

                Assert.NotEqual(Guid.Empty, mapPoint.Id);
            }

            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                var fetchedMapPoint = await mapRepository.FindByIdAsync(mapPoint.Id);
                Assert.Equal(mapPoint.Name, fetchedMapPoint.Name);
                Assert.Equal(mapPoint.Latitude, fetchedMapPoint.Latitude);
                Assert.Equal(mapPoint.Longitude, fetchedMapPoint.Longitude);

                _mapPointId = fetchedMapPoint.Id;
            }
        }

        [Fact, TestPriority(2)]
        public async Task ShouldDeleteEntry()
        {
            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                await mapService.RemoveByIdAsync(_mapPointId);
            }

            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                var mapPoint = await mapService.FindByIdAsync(_mapPointId);

                Assert.Null(mapPoint);
            }
        }

        public void Dispose()
        {
            _contextFactory.Dispose();
        }
    }
}