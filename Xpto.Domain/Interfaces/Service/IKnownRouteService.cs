using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service.Common;

namespace Xpto.Domain.Interfaces.Service
{
    public interface IKnownRouteService : IService<KnownRoute>
    {
        Task<KnownRoute> FindBestRouteAsync(Guid mapPoint1Id, Guid mapPoint2Id);
        Task<IEnumerable<KnownRoute>> GetAllFullAsync();
    }
}