using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context;
using Xpto.Data.Context.Interfaces;
using Xpto.Data.Repository.EntityFramework.Common;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;

namespace Xpto.Data.Repository.EntityFramework
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        public RouteRepository(XptoContext context)
            : base(context)
        {
        }
    }
}