using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context;
using Xpto.Data.Context.Interfaces;
using Xpto.Data.Repository.EntityFramework;
using Xpto.Domain.Entities;
using Xpto.Domain.Services;
using Xpto.Tests.Common;
using Xunit;

namespace tests.serviceTests
{
    [TestCaseOrderer("Xpto.Tests.Common.PriorityOrderer", "Xpto.Tests")]
    public class RouteServiceTests
    {
        private IContextFactory<XptoContext> _contextFactory;
        private Guid _routeId;
        private Guid _mapPoint1Id;
        private Guid _mapPoint2Id;
        private Guid _mapPoint3Id;

        public RouteServiceTests()
        {
            _contextFactory = new ContextFactory<XptoContext>();

            using (var context = _contextFactory.CreateDbContext())
            {
                var mapPointRepository = new MapPointRepository(context);

                var routePoint1 = new MapPoint("Test Point 1", -43.45554434, -110.04886744);
                var routePoint2 = new MapPoint("Test Point 2", -43.45559877, -110.03486744);
                var routePoint3 = new MapPoint("Test Point 3", -43.44599434, -110.12880004);

                var addPointTask1 = mapPointRepository.AddAsync(routePoint1);
                var addPointTask2 = mapPointRepository.AddAsync(routePoint1);
                var addPointTask3 = mapPointRepository.AddAsync(routePoint1);
                Task.WaitAll(addPointTask1, addPointTask2, addPointTask3);
                Task.WaitAll(mapPointRepository.SaveChangesAsync());

                _mapPoint1Id = routePoint1.Id;
                _mapPoint2Id = routePoint2.Id;
                _mapPoint3Id = routePoint3.Id;
            }
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            using (var context = _contextFactory.CreateDbContext())
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
            var route = new Route(_mapPoint1Id, _mapPoint2Id);
            using (var context = _contextFactory.CreateDbContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

                await routeService.AddAsync(route);

                Assert.NotEqual(Guid.Empty, route.Id);
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

                var fetchedRoute = await routeRepository.GetByIdAsync(route.Id);
                Assert.Equal(route.FromId, fetchedRoute.FromId);
                Assert.Equal(route.ToId, fetchedRoute.ToId);

                _routeId = fetchedRoute.Id;
            }
        }

        [Fact, TestPriority(2)]
        public async Task ShouldDeleteEntry()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

                await routeService.RemoveByIdAsync(_routeId);
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                var routeRepository = new RouteRepository(context);
                var routeService = new RouteService(routeRepository);

                var route = await routeService.GetByIdAsync(_routeId);

                Assert.Null(route);
            }
        }
    }
}