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
    public class RouteRepositoryTests : IDisposable
    {
        private readonly MainContextFactory _contextFactory;

        private Guid _routeId;
        private Guid _mapPoint1Id;
        private Guid _mapPoint2Id;
        private Guid _mapPoint3Id;

        public RouteRepositoryTests()
        {
            _contextFactory = new MainContextFactory();

            using (var context = _contextFactory.CreateContext())
            {
                var mapPointRepository = new MapPointRepository(context);
                
                var mapPoint1 = new MapPointEntity("Test Point 1", -43.45554434, -110.04886744);
                var mapPoint2 = new MapPointEntity("Test Point 2", -43.45559877, -110.03486744);
                var mapPoint3 = new MapPointEntity("Test Point 3", -43.44599434, -110.12880004);

                var addPointTask1 = mapPointRepository.AddAsync(mapPoint1);
                var addPointTask2 = mapPointRepository.AddAsync(mapPoint1);
                var addPointTask3 = mapPointRepository.AddAsync(mapPoint1);
                Task.WaitAll(addPointTask1, addPointTask2, addPointTask3);
                Task.WaitAll(mapPointRepository.SaveChangesAsync());

                _mapPoint1Id = mapPoint1.Id;
                _mapPoint2Id = mapPoint2.Id;
                _mapPoint3Id = mapPoint3.Id;
            }
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);
                var map = new MapPointRepository(context);

                var mapPoints = await routeRepository.GetAllAsync();

                Assert.Empty(mapPoints);
            }
        }

        [Fact, TestPriority(1)]
        public async Task ShouldCreateEntry()
        {
            var route = new RouteEntity(_mapPoint1Id, _mapPoint2Id);
            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);

                await routeRepository.AddAsync(route);
                await routeRepository.SaveChangesAsync();

                Assert.NotEqual(Guid.Empty, route.Id);
            }

            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);

                var fetchedRoute = await routeRepository.FindByIdAsync(route.Id);
                Assert.Equal(route.FromId, fetchedRoute.FromId);
                Assert.Equal(route.ToId, fetchedRoute.ToId);

                _routeId = fetchedRoute.Id;
            }
        }

        [Fact, TestPriority(2)]
        public async Task ShouldDeleteEntry()
        {
            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);

                await routeRepository.RemoveByAsync(mp => mp.Id.Equals(_routeId));
                await routeRepository.SaveChangesAsync();
            }

            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);

                var route = await routeRepository.FindByIdAsync(_routeId);

                Assert.Null(route);
            }
        }

        public void Dispose()
        {
            _contextFactory.Dispose();
        }
    }
}