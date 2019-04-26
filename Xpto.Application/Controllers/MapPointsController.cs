using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xpto.Domain.Interfaces.Service;
using Xpto.Domain.DTO;

namespace Xpto.Application.Controllers
{
    [Route("api/[controller]")]
    public class MapPointsController : ControllerBase
    {
        private IMapPointService _mapPointService;
        public MapPointsController(IMapPointService mapPointService)
        {
            _mapPointService = mapPointService;
        }

        // GET api/mappoints
        [HttpGet]
        /// <summary>
        /// Fetch all map points from the database
        /// </summary>
        /// <returns>List of mappoints</returns>
        public async Task<ActionResult<List<MapPointDTO>>> Get()
        {
            var mapPoints = await _mapPointService.GetAllAsync();
            if (mapPoints.Count() == 0)
                return NoContent();

            var mapPointsDTO = new List<MapPointDTO>();
            foreach (var mapPoint in mapPoints)
            {
                mapPointsDTO.Add(new MapPointDTO(mapPoint));
            }

            return Ok(mapPointsDTO);
        }

        // api/mappoints/000000000-0000-0000-0000-000000000000
        [HttpGet("{mapPointId}")]

        public async Task<ActionResult<MapPointDTO>> Get(Guid mapPointId)
        {
            var mapPoint = await _mapPointService.GetByIdAsync(mapPointId);
            if (mapPoint == null)
                return NotFound("MapPoint not found.");

            return Ok(new MapPointDTO(mapPoint));
        }

        [HttpPost]
        public async Task<ActionResult<MapPointDTO>> Create(MapPointDTO mapPointDTO)
        {
            mapPointDTO.Name = "a";
            mapPointDTO.Latitude = 123;
            mapPointDTO.Longitude = 12;
            //var mapPoint = mapPointDTO.ToEntity();
            //await _mapPointService.AddAsync(mapPoint);

            return Ok(mapPointDTO);//new MapPointDTO(mapPoint));
        }

        [HttpPost("{mapPointId}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(Guid mapPointId, MapPointDTO mapPointDTO)
        {
            if (mapPointId == null || mapPointId == Guid.Empty)
                return StatusCode(500, $"{nameof(mapPointId)} can't be empty.");
            
            if (mapPointDTO.Id != null && mapPointDTO.Id != Guid.Empty && mapPointDTO.Id != mapPointId)
                return StatusCode(500, $"{nameof(mapPointId)} invalid.");
            
            var mapPoint = mapPointDTO.ToEntity();
            mapPoint.Id = mapPointId;

            await _mapPointService.UpdateAsync(mapPoint);

            return Ok(new MapPointDTO(mapPoint));
        }
    }
}