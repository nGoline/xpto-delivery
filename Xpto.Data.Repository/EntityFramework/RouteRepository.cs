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
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        public RouteRepository(XptoContext context) 
            : base(context) { }

        public async Task<IEnumerable<Route>> GetAllFullAsync()
        {
            return await DbSet.Include(r => r.From)
                .Include(r => r.To)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task AddAsync(Route entity)
        {
            entity.From = null;
            entity.To = null;
            await DbSet.AddAsync(entity).ConfigureAwait(false);
        }
    }
}
