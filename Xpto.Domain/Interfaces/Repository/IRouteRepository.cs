using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository.Common;

namespace Xpto.Domain.Interfaces.Repository
{
    public interface IRouteRepository : IRepository<Route>
    {      
        Task<IEnumerable<Route>> GetAllFullAsync();
    }
}