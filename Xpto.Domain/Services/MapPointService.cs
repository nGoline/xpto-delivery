using System;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services.Common;

namespace Xpto.Domain.Services
{
    public class MapPointService : Service<MapPoint>, IMapPointService
    {
        public MapPointService(IMapPointRepository repository)
            : base(repository)
        { }

        public async Task DeleteByIdAsync(Guid mapPointId)
        {
            await Repository.DeleteByAsync(mp => mp.Id.Equals(mapPointId));
        }
    }
}