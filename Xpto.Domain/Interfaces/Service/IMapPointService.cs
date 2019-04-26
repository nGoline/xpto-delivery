using System;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Service.Common;

namespace Xpto.Domain.Interfaces.Service
{
    public interface IMapPointService : IService<MapPoint>
    {
         Task DeleteByIdAsync(Guid mapPointId);
    }
}