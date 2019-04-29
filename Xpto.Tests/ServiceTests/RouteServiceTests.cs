using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context;
using Xpto.Data.Context.Interfaces;
using Xpto.Data.Repository.EntityFramework;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services;
using Xpto.Tests.Common;
using Xpto.Tests.Fake;
using Xunit;

namespace Xpto.Tests.ServiceTests
{
    [TestCaseOrderer("Xpto.Tests.Common.PriorityOrderer", "Xpto.Tests")]
    public class RouteServiceTests
    {
        private static IRouteRepository _repository;
        private static IRouteService _service;
        private static IMapPointService _mapPointService;
        private static Guid _routeId;

        public RouteServiceTests()
        {
            if (_repository == null)
                _repository = new FakeRouteRepository();
            
            if (_mapPointService == null)
                _mapPointService = new FakeMapPointService();

            if (_service == null)
                _service = new RouteService(_repository, _mapPointService);
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            var routes = await _service.GetAllAsync();

            Assert.Empty(routes);
        }

        [Fact, TestPriority(1)]
        public async Task ShouldCreateEntry()
        {
            var route = new Route(Guid.NewGuid(), Guid.NewGuid(), 10, 10);
            await _service.AddAsync(route);

            Assert.NotEqual(Guid.Empty, route.Id);

            var fetchedRoute = await _service.GetByIdAsync(route.Id);
            Assert.Equal(route.FromId, fetchedRoute.FromId);
            Assert.Equal(route.ToId, fetchedRoute.ToId);

            _routeId = fetchedRoute.Id;
        }

        [Fact, TestPriority(2)]
        public async Task ShouldDeleteEntry()
        {
            await _service.DeleteByIdAsync(_routeId);
            var route = await _service.GetByIdAsync(_routeId);

            Assert.Null(route);
        }
    }
}