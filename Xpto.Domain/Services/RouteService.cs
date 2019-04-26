using System;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services.Common;

namespace Xpto.Domain.Services
{
    public class RouteService : Service<Route>, IRouteService
    {
        public RouteService(IRouteRepository repository)
            : base(repository)
        { }

        public async Task RemoveByIdAsync(Guid routeId)
        {
            await Repository.DeleteByAsync(r=>r.Id.Equals(routeId));
        }
    }
}