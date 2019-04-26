using System;
using System.Threading.Tasks;
using Xpto.Domain.Entities;
using Xpto.Domain.Interfaces.Repository;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Services.Common;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Services
{
    public class MapPointService : Service<MapPoint>, IMapPointService
    {
        private IRouteService _routeService;
        public MapPointService(IMapPointRepository repository, IRouteService routeService)
            : base(repository)
        {
            _routeService = routeService;
        }

        public async Task DeleteByIdAsync(Guid mapPointId)
        {
            await Repository.DeleteByAsync(mp => mp.Id.Equals(mapPointId));
        }

        public override async Task<ValidationResult> AddAsync(MapPoint entity)
        {
            if (!ValidationResult.IsValid)
                return ValidationResult;

            var selfValidationEntity = entity as ISelfValidation;
            if (selfValidationEntity != null && !selfValidationEntity.IsValid)
                return selfValidationEntity.ValidationResult;

            var existingMapPoints = await Repository.GetAllAsync();

            await Repository.AddAsync(entity);

            await _routeService.CreateRoutesUsingNewPoint(existingMapPoints, entity);

            return ValidationResult;
        }
    }
}