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
    public class RoutesController : ControllerBase
    {
        private IRouteService _routeService;
        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        // GET api/route
        [HttpGet]
        /// <summary>
        /// Fetch all map points from the database
        /// </summary>
        /// <returns>List of routes</returns>
        public async Task<ActionResult<List<RouteDTO>>> Get()
        {
            var routes = await _routeService.GetAllAsync();
            if (routes.Count() == 0)
                return NoContent();

            var routesDTO = new List<RouteDTO>();
            foreach (var route in routes)
            {
                routesDTO.Add(new RouteDTO(route));
            }

            return Ok(routesDTO);
        }

        // api/mappoints/000000000-0000-0000-0000-000000000000
        [HttpGet("{routeId}")]

        public async Task<ActionResult<RouteDTO>> Get(Guid routeId)
        {
            var route = await _routeService.GetByIdAsync(routeId);
            if (route == null)
                return NotFound("MapPoint not found.");

            return Ok(new RouteDTO(route));
        }

        [HttpPost]
        public async Task<ActionResult> Create(RouteDTO routeDTO)
        {
            var mapPoint = routeDTO.ToEntity();

            if (!mapPoint.IsValid)
            {
                return BadRequest(mapPoint.ValidationResult);
            }

            await _routeService.AddAsync(mapPoint);

            return base.CreatedAtAction("Get", new { Id = mapPoint.Id }, new RouteDTO(mapPoint));
        }

        [HttpPut("{routeId}")]
        public async Task<ActionResult<UserDTO>> UpdateRoute(Guid routeId, RouteDTO routeDTO)
        {
            if (routeId == null || routeId == Guid.Empty)
                return StatusCode(400, $"{nameof(routeId)} can't be empty.");

            if (routeDTO.Id != null && routeDTO.Id != Guid.Empty && routeDTO.Id != routeId)
                return StatusCode(400, $"{nameof(routeId)} invalid.");

            var mapPoint = routeDTO.ToEntity();
            mapPoint.Id = routeId;

            await _routeService.UpdateAsync(mapPoint);

            return base.Ok(new RouteDTO(mapPoint));
        }

        [HttpDelete("{routeId}")]
        public async Task<ActionResult> DeleteRoute(Guid routeId)
        {
            if (routeId == null || routeId == Guid.Empty)
                return StatusCode(400, $"{nameof(routeId)} can't be empty.");

            var route = await _routeService.GetByIdAsync(routeId);
            if (route != null)
                return NotFound("Route not found.");

            await _routeService.DeleteAsync(route);

            return Ok();
        }
    }
}