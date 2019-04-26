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
        private static Random _rnd = new Random(123);
        private Dictionary<Guid, MapPoint> _mapPoints = new Dictionary<Guid, MapPoint>();

        public MapPoint GetNewMapPoint(Guid? id = null)
        {
            if (id == null)
                id = Guid.NewGuid();

            var mapPoint = new MapPoint
            {
                Id = id.Value,
                Name = $"Test {id.ToString()}",
                Latitude = ((_rnd.Next(1) == 1 ? 1 : -1) * _rnd.NextDouble() * 89) + 1,
                Longitude = ((_rnd.Next(1) == 1 ? 1 : -1) * _rnd.NextDouble() * 179) + 1
            };

            _mapPoints.Add(mapPoint.Id, mapPoint);

            return mapPoint;
        }

        public Task AddAsync(MapPoint entity)
        {
            entity.Id = Guid.NewGuid();
            _mapPoints.Add(entity.Id, entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MapPoint entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByAsync(Func<MapPoint, bool> where)
        {
            var toDelete = _mapPoints.Values.Where(where).ToList();
            foreach (var mapPoint in toDelete)
                _mapPoints.Remove(mapPoint.Id);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<MapPoint>> FindAsync(Expression<Func<MapPoint, bool>> predicate, bool asNoTracking = true, Expression<Func<MapPoint, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MapPoint>> GetAllAsync(bool asNoTracking = true, Expression<Func<MapPoint, object>> include = null)
        {
            return Task.FromResult((IEnumerable<MapPoint>)_mapPoints.Values);
        }

        public Task<MapPoint> GetByIdAsync(Guid id, bool asNoTracking = true, Expression<Func<MapPoint, object>> include = null)
        {
            return Task.FromResult(_mapPoints.ContainsKey(id) ? _mapPoints[id] : null);
        }

        public Task UpdateAsync(MapPoint entity)
        {
            throw new NotImplementedException();
        }
    }
}