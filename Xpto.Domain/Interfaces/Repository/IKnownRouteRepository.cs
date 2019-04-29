using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository.Common;

namespace Xpto.Domain.Interfaces.Repository
{
    public interface IKnownRouteRepository : IRepository<KnownRoute>
    {
        Task<KnownRoute> FindBestRouteAsync(Guid mapPoint1Id, Guid mapPoint2Id);
        Task<IEnumerable<KnownRoute>> GetAllFullAsync();
    }
}
