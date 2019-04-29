using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;

namespace Xpto.Tests.Fake
{
    public class FakeRouteRepository : IRouteRepository
    {
        private Dictionary<Guid, Route> _routes = new Dictionary<Guid, Route>();

        private IMapPointRepository _fakeMapPointRepository;

        public FakeRouteRepository(IMapPointRepository fakeMapPointRepository = null)
        {
            if (fakeMapPointRepository == null)
                fakeMapPointRepository = new FakeMapPointRepository();

            _fakeMapPointRepository = fakeMapPointRepository;
        }

        public async Task AddAsync(Route entity)
        {
            if (entity.Id == null || entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            if ((await _fakeMapPointRepository.GetByIdAsync(entity.FromId)) == null)
            {
                if (entity.From == null)
                    entity.From = ((FakeMapPointRepository)_fakeMapPointRepository).GetNewMapPoint(entity.FromId);

                await _fakeMapPointRepository.AddAsync(entity.From);
            }

            if ((await _fakeMapPointRepository.GetByIdAsync(entity.ToId)) == null)
            {
                if (entity.To == null)
                    entity.To = ((FakeMapPointRepository)_fakeMapPointRepository).GetNewMapPoint(entity.ToId);

                await _fakeMapPointRepository.AddAsync(entity.To);
            }

            _routes.Add(entity.Id, entity);
        }

        public Task DeleteAsync(Route entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByAsync(Func<Route, bool> where)
        {
            var routes = _routes.Values.Where(where).ToList();
            foreach (var route in routes)
                _routes.Remove(route.Id);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Route>> FindAsync(Expression<Func<Route, bool>> predicate, bool asNoTracking = true, Expression<Func<Route, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> FindBestRouteAsync(Guid mapPoint1Id, Guid mapPoint2Id)
        {
            var initial = _routes.Values
                // Get all mapPoints that has the first id but not the second id
                .Where(r => (r.ToId.Equals(mapPoint1Id) || r.FromId.Equals(mapPoint1Id))
                         && (r.FromId != mapPoint2Id || r.FromId != mapPoint2Id))
                .Select(r => new
                {
                    FullPath = r.ToId.Equals(mapPoint1Id)
                        ? new List<Guid> { r.FromId, r.ToId }
                        : new List<Guid> { r.ToId, r.FromId },
                    Cost = r.Cost,
                    Time = r.Time
                }).ToList();

            var visited = initial.SelectMany(r => r.FullPath).ToList();

            for (int i = 0; i < initial.Count(); i++)
            {
                var nextEl = initial.ElementAt(i);
                var iteration = _routes.Values
                    .Where(r => !visited.Contains(r.Id) && (r.ToId.Equals(nextEl.FullPath.Last()) || r.FromId.Equals(nextEl.FullPath.Last())))
                    .Select(r => new
                    {
                        FullPath = nextEl.FullPath.Append(r.ToId.Equals(mapPoint1Id)
                            ? r.ToId
                            : r.FromId).ToList(),
                        Cost = nextEl.Cost + r.Cost,
                        Time = nextEl.Time + r.Time
                    }).ToList();

                initial.AddRange(iteration);

                visited.AddRange(iteration.Select(r => r.FullPath.Last()));
            }

            return Task.FromResult(initial.OrderBy(r => r.Time)
                .FirstOrDefault(r => r.FullPath.First().Equals(mapPoint1Id)
                                  && r.FullPath.Last().Equals(mapPoint2Id))
                .FullPath);
        }

        public async Task<IEnumerable<Route>> GetAllAsync(bool asNoTracking = true, Expression<Func<Route, object>> include = null)
        {
            var routes = new List<Route>();
            foreach (var route in _routes.Values)
            {
                route.From = await _fakeMapPointRepository.GetByIdAsync(route.FromId);
                route.To = await _fakeMapPointRepository.GetByIdAsync(route.ToId);
            }

            return routes;
        }

    public Task<IEnumerable<Route>> GetAllFullAsync()
    {
      throw new NotImplementedException();
    }

    public async Task<Route> GetByIdAsync(Guid id, bool asNoTracking = true, Expression<Func<Route, object>> include = null)
        {
            if (!_routes.ContainsKey(id))
                return null;

            var route = _routes[id];
            if (route != null)
            {
                route.From = await _fakeMapPointRepository.GetByIdAsync(route.FromId);
                route.To = await _fakeMapPointRepository.GetByIdAsync(route.ToId);
            }

            return route;
        }

        public Task UpdateAsync(Route entity)
        {
            _routes[entity.Id] = entity;

            return Task.CompletedTask;
        }
    }
}