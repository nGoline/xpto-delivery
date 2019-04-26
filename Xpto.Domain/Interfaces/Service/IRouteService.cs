using System;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service.Common;

namespace Xpto.Domain.Interfaces.Service
{
    public interface IRouteService : IService<Route>
    {
         Task RemoveByIdAsync(Guid routeId);
    }
}