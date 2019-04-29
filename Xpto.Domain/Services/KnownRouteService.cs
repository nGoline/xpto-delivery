using System.Security.AccessControl;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Services.Common;
using System.Collections.Generic;
using Xpto.Domain.Validation;
using System.Linq.Expressions;
using Xpto.Domain.DTO;

namespace Xpto.Domain.Services
{
    public class KnownRouteService : Service<KnownRoute>, IKnownRouteService
    {
        private IKnownRouteRepository _repository;
        private IMapPointService _mapPointService;

        public KnownRouteService(IKnownRouteRepository repository, IMapPointService mapPointService)
            : base(repository)
        {
            _repository = repository;
            _mapPointService = mapPointService;
        }

        public async Task<KnownRoute> FindBestRouteAsync(Guid mapPoint1Id, Guid mapPoint2Id)
        {
            var knownRoute = await _repository.FindBestRouteAsync(mapPoint1Id, mapPoint2Id);
            knownRoute.MapPoints = new List<MapPoint>(knownRoute.MapPointIds.Count);

            foreach (var mapPointId in knownRoute.MapPointIds)                
                knownRoute.MapPoints.Add(await _mapPointService.GetByIdAsync(mapPointId));

            return knownRoute;
        }

        public async Task<IEnumerable<KnownRoute>> GetAllFullAsync()
        {
            return await _repository.GetAllFullAsync();
        }
    }
}