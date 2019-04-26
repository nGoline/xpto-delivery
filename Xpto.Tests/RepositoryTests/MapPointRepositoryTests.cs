using System;
using System.Linq;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xpto.Tests.Common;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Data.Repository.EntityFramework;
using Xpto.Data.Context.Interfaces;
using Xpto.Data.Context;

namespace Xpto.Tests.RepositoryTests
{
    [TestCaseOrderer("Xpto.Tests.Common.PriorityOrderer", "Xpto.Tests")]
    public class MapPointRepositoryTests : IDisposable
    {
        private IContextFactory<XptoContext> _contextFactory;
        private Guid _mapPointId;

        public MapPointRepositoryTests()
        {
            _contextFactory = new ContextFactory<XptoContext>();
        }

        [Fact, TestPriority(0)]
        public async Task DatabaseIsEmpty()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var mapRepository = new MapPointRepository(context);

                var mapPoints = await mapRepository.GetAllAsync();

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

                await mapRepository.AddAsync(mapPoint);
                await mapRepository.SaveChangesAsync();

                Assert.NotEqual(Guid.Empty, mapPoint.Id);
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                var mapRepository = new MapPointRepository(context);

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

                await mapRepository.DeleteByAsync(mp => mp.Id.Equals(_mapPointId));
                await mapRepository.SaveChangesAsync();
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                var mapRepository = new MapPointRepository(context);

                var mapPoint = await mapRepository.GetByIdAsync(_mapPointId);

                Assert.Null(mapPoint);
            }
        }
        public void Dispose()
        {
            _contextFactory.Dispose();
        }
    }
}