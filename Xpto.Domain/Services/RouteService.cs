using System.Security.AccessControl;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services.Common;
using System.Collections.Generic;

namespace Xpto.Domain.Services
{
    public class RouteService : Service<Route>, IRouteService
    {
        public RouteService(IRouteRepository repository)
            : base(repository)
        { }

        public async Task RemoveByIdAsync(Guid routeId)
        {
            await Repository.DeleteByAsync(r => r.Id.Equals(routeId));
        }

        public async Task UpdateRoutesContainingMapPoint(MapPoint mapPoint)
        {
            var routes = await Repository.FindAsync(r => r.FromId.Equals(mapPoint.Id)
                                                || r.ToId.Equals(mapPoint.Id));

            foreach (var route in routes)
            {
                if (route.FromId == mapPoint.Id)
                    route.Cost = mapPoint.Coordinate.GetDistanceTo(route.To.Coordinate);
                else
                    route.Cost = route.From.Coordinate.GetDistanceTo(mapPoint.Coordinate);

                await Repository.UpdateAsync(route);
            }
        }

        public async Task CreateRoutesUsingNewPoint(IEnumerable<MapPoint> mapPoints, MapPoint newMapPoint)
        {
            foreach (var mapPoint in mapPoints)
            {
                var route = new Route(mapPoint.Id, newMapPoint.Id)
                {
                    Cost = mapPoint.Coordinate.GetDistanceTo(newMapPoint.Coordinate)
                };

                await Repository.AddAsync(route);
            }
        }
    }
}