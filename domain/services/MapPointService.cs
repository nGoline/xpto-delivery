using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using domain.entities;
using domain.repositories;

namespace domain.services
{
    public class MapPointService
    {
        private readonly MapPointRepository _mapPointRepository;

        /// <summary>
        /// Service constructor takes Repositories needed from IoC injection
        /// </summary>
        /// <param name="mapPointRepository">MapPointRepository object</param>
        public MapPointService(MapPointRepository mapPointRepository)
        {
            _mapPointRepository = mapPointRepository;
        }

        /// <summary>
        /// Gets all MapPoints
        /// </summary>
        /// <returns>Users list</returns>
        public async Task<IEnumerable<MapPointEntity>> GetAllMapPointsAsync()
        {
            return await _mapPointRepository.GetAllAsync(true);
        }

        public async Task<MapPointEntity> FindMapPointByIdAsync(Guid id)
        {
            return await _mapPointRepository.FindMapPointByIdAsync(id);
        }
    }
}