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
    public class RouteServiceTests : IDisposable
    {
        private readonly MainContextFactory _contextFactory;

        private Guid _routeId;
        private Guid _mapPoint1Id;
        private Guid _mapPoint2Id;
        private Guid _mapPoint3Id;

        public RouteServiceTests()
        {
            _contextFactory = new MainContextFactory();

            using (var context = _contextFactory.CreateContext())
            {
                var routePointRepository = new MapPointRepository(context);

                var routePoint1 = new MapPointEntity("Test Point 1", -43.45554434, -110.04886744);
                var routePoint2 = new MapPointEntity("Test Point 2", -43.45559877, -110.03486744);
                var routePoint3 = new MapPointEntity("Test Point 3", -43.44599434, -110.12880004);

                var addPointTask1 = routePointRepository.AddAsync(routePoint1);
                var addPointTask2 = routePointRepository.AddAsync(routePoint1);
                var addPointTask3 = routePointRepository.AddAsync(routePoint1);
                Task.WaitAll(addPointTask1, addPointTask2, addPointTask3);
                Task.WaitAll(routePointRepository.SaveChangesAsync());

                _mapPoint1Id = routePoint1.Id;
                _mapPoint2Id = routePoint2.Id;
                _mapPoint3Id = routePoint3.Id;
            }
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

                var routes = await routeService.GetAllAsync();

                Assert.Empty(routes);
            }
        }

        [Fact, TestPriority(1)]
        public async Task ShouldCreateEntry()
        {
            var route = new RouteEntity(_mapPoint1Id, _mapPoint2Id);
            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

                await routeService.AddAsync(route);

                Assert.NotEqual(Guid.Empty, route.Id);
            }

            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

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
                var routeService = new RouteService(routeRepository);

                await routeService.RemoveByIdAsync(_routeId);
            }

            using (var context = _contextFactory.CreateContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

                var route = await routeService.FindByIdAsync(_routeId);

                Assert.Null(route);
            }
        }

        public void Dispose()
        {
            _contextFactory.Dispose();
        }
    }
}