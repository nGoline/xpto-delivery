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

namespace Xpto.Tests.ServiceTests
{
    [TestCaseOrderer("Xpto.Tests.Common.PriorityOrderer", "Xpto.Tests")]
    public class MapPointServiceTests
    {
        private IContextFactory<XptoContext> _contextFactory;
        private Guid _mapPointId;

        public MapPointServiceTests()
        {
            _contextFactory = new ContextFactory<XptoContext>();
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            using (var context = _contextFactory.CreateDbContext())
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
            var mapPoint = new MapPoint("Test Point 1", -43.45554434, -110.04886744);
            using (var context = _contextFactory.CreateDbContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                await mapService.AddAsync(mapPoint);

                Assert.NotEqual(Guid.Empty, mapPoint.Id);
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                var fetchedMapPoint = await mapRepository.GetByIdAsync(mapPoint.Id);
                Assert.Equal(mapPoint.Name, fetchedMapPoint.Name);
                Assert.Equal(mapPoint.Latitude, fetchedMapPoint.Latitude);
                Assert.Equal(mapPoint.Longitude, fetchedMapPoint.Longitude);

                _mapPointId = fetchedMapPoint.Id;
            }
        }

        [Fact, TestPriority(2)]
        public async Task ShouldDeleteEntry()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                await mapService.DeleteByIdAsync(_mapPointId);
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                var mapRepository = new MapPointRepository(context);
                var mapService = new MapPointService(mapRepository);

                var mapPoint = await mapService.GetByIdAsync(_mapPointId);

                Assert.Null(mapPoint);
            }
        }
    }
}