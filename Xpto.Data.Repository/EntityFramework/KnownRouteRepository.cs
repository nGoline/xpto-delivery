using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Xpto.Data.Context;
using Xpto.Data.Context.Interfaces;
using Xpto.Data.Repository.EntityFramework.Common;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;

namespace Xpto.Data.Repository.EntityFramework
{
    public class KnownRouteRepository : Repository<KnownRoute>, IKnownRouteRepository
    {
        private IRouteRepository routeRepository;
        public KnownRouteRepository(XptoContext context, IRouteRepository _routeRepository) 
            : base(context)
        {
            routeRepository = _routeRepository;
        }

        /// <summary>
        /// Find best route using A* Algorithm
        /// </summary>
        /// <param name="mapPoint1Id"></param>
        /// <param name="mapPoint2Id"></param>
        /// <returns></returns>
        public async Task<KnownRoute> FindBestRouteAsync(Guid mapPoint1Id, Guid mapPoint2Id)
        {
            var routes = await routeRepository
                .FindAsync(r => (r.ToId.Equals(mapPoint1Id) || r.FromId.Equals(mapPoint1Id)) &&
                          (r.FromId != mapPoint2Id || r.FromId != mapPoint2Id));

            var initial = routes.Select(r => new
            {
                RouteId = r.Id,
                FullPath = r.ToId.Equals(mapPoint1Id) ?
                    new List<Guid> { r.ToId, r.FromId } :
                    new List<Guid> { r.FromId, r.ToId },
                Cost = r.Cost,
                Time = r.Time
            }).ToList();

            var visited = initial.Select(r => r.RouteId).Distinct().ToList();

            for (int i = 0; i < initial.Count(); i++)
            {
                var nextEl = initial.ElementAt(i);
                routes = await routeRepository
                    .FindAsync(r => !visited.Contains(r.Id) &&
                               (r.ToId.Equals(nextEl.FullPath.Last()) || r.FromId.Equals(nextEl.FullPath.Last())) &&
                               !nextEl.FullPath.Take(nextEl.FullPath.Count() - 1).Contains(r.ToId) &&
                               !nextEl.FullPath.Take(nextEl.FullPath.Count() - 1).Contains(r.FromId));
                
                var iteration = routes.Select(r => new
                    {
                        RouteId = r.Id,
                            FullPath = nextEl.FullPath.Append(r.ToId.Equals(nextEl.FullPath.Last()) ?
                                r.FromId :
                                r.ToId).ToList(),
                            Cost = nextEl.Cost + r.Cost,
                            Time = nextEl.Time + r.Time
                    }).ToList();

                initial.AddRange(iteration);

                visited.AddRange(iteration.Select(r => r.RouteId));
            }

            var chosenRoute = initial.OrderBy(r => r.Time)
                .FirstOrDefault(r => r.FullPath.First().Equals(mapPoint1Id) &&
                                     r.FullPath.Last().Equals(mapPoint2Id));

            if (chosenRoute == null)
                return null;
            
            return new KnownRoute(chosenRoute.FullPath, chosenRoute.Time, chosenRoute.Cost);
        }

        public async Task<IEnumerable<KnownRoute>> GetAllFullAsync()
        {
            return await DbSet.Include(r => r.MapPoints)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
