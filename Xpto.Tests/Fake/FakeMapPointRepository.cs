using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;

namespace Xpto.Tests.Fake
{
    public class FakeMapPointRepository : IMapPointRepository
    {
        private List<MapPoint> _mapPoints = new List<MapPoint>{
            new MapPoint("Test 1", 10, -10){ Id = Guid.NewGuid() },
            new MapPoint("Test 2", 11, -11){ Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200") },
            new MapPoint("Test 3", 13, -13){ Id = Guid.NewGuid() }
        };

        public Task AddAsync(MapPoint entity)
        {
            _mapPoints.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MapPoint entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByAsync(Func<MapPoint, bool> where)
        {
            foreach(var mapPoint in _mapPoints.Where(where))
                _mapPoints.Remove(mapPoint);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<MapPoint>> FindAsync(Expression<Func<MapPoint, bool>> predicate, bool asNoTracking = true, Expression<Func<MapPoint, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MapPoint>> GetAllAsync(bool asNoTracking = true, Expression<Func<MapPoint, object>> include = null)
        {
            return Task.FromResult((IEnumerable<MapPoint>)_mapPoints);
        }

        public Task<MapPoint> GetByIdAsync(Guid id, bool asNoTracking = true, Expression<Func<MapPoint, object>> include = null)
        {
            return Task.FromResult(_mapPoints.SingleOrDefault(mp=>mp.Id.Equals(id)));
        }

        public Task UpdateAsync(MapPoint entity)
        {
            throw new NotImplementedException();
        }
    }
}