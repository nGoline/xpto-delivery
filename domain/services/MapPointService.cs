using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using domain.entities;
using domain.repositories;

namespace domain.services
{
    public class MapPointService : ServiceBase<MapPointEntity>
    {
        private MapPointRepository _mapPointRepository;

        /// <summary>
        /// Service constructor takes Repositories needed from IoC injection
        /// </summary>
        /// <param name="mapPointRepository">MapPointRepository object</param>
        public MapPointService(MapPointRepository mapPointRepository)
            : base(mapPointRepository)
        {
            _mapPointRepository = mapPointRepository;
        }

        public async Task UpdateAsync(MapPointEntity mapPoint)
        {
            var oldMapPoint = await _mapPointRepository.FindByIdAsync(mapPoint.Id);
            if (oldMapPoint.Name != mapPoint.Name)
                oldMapPoint.Name = mapPoint.Name;
            
            if (oldMapPoint.Name != mapPoint.Name)
                oldMapPoint.Name = mapPoint.Name;

            if (oldMapPoint.Latitude != mapPoint.Latitude)
                oldMapPoint.Latitude = mapPoint.Latitude;

            if (oldMapPoint.Longitude != mapPoint.Longitude)
                oldMapPoint.Longitude = mapPoint.Longitude;

            oldMapPoint.Validate(false);

            await _mapPointRepository.UpdateAsync(oldMapPoint);
            await _mapPointRepository.SaveChangesAsync();
        }
    }
}