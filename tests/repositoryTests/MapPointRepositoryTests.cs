using System;
using System.Linq;
using System.Threading.Tasks;
using domain.contexts;
using domain.entities;
using domain.repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using tests.helpers;
using Xunit;

namespace tests.repositoryTests
{
    
    [TestCaseOrderer("tests.helpers.PriorityOrderer", "tests")]
    public class MapPointRepositoryTests : IDisposable
    {
        private readonly MainContextFactory _contextFactory;
        
        private Guid _mapPointId;

        public MapPointRepositoryTests()
        {
            _contextFactory = new MainContextFactory();
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);

                var mapPoints = await mapRepository.GetAllAsync();

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

                await mapRepository.AddAsync(mapPoint);
                await mapRepository.SaveChangesAsync();

                Assert.NotEqual(Guid.Empty, mapPoint.Id);
            }

            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);

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

                await mapRepository.RemoveByAsync(mp => mp.Id.Equals(_mapPointId));
                await mapRepository.SaveChangesAsync();
            }

            using (var context = _contextFactory.CreateContext())
            {
                var mapRepository = new MapPointRepository(context);

                var mapPoint = await mapRepository.FindByIdAsync(_mapPointId);

                Assert.Null(mapPoint);
            }
        }

        public void Dispose()
        {
            _contextFactory.Dispose();
        }
    }
}