using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Xpto.Domain.DTO;
using Xpto.Domain.Interfaces.Service;

namespace Xpto.Application.Controllers
{
    [Route("api/[controller]")]
    public class KnownRoutesController : ControllerBase
    {
        private IKnownRouteService _knownRouteService;
        public KnownRoutesController(IKnownRouteService knownRouteService)
        {
            _knownRouteService = knownRouteService;
        }

        // GET api/route
        [HttpGet]
        /// <summary>
        /// Fetch all map points from the database
        /// </summary>
        /// <returns>List of routes</returns>
        public async Task<ActionResult<List<KnownRouteDTO>>> Get()
        {
            var routes = await _knownRouteService.GetAllFullAsync();
            if (routes.Count() == 0)
                return NoContent();

            var routesDTO = new List<KnownRouteDTO>();
            foreach (var route in routes)
                routesDTO.Add(new KnownRouteDTO(route));

            return Ok(routesDTO);
        }

        // api/mappoints/000000000-0000-0000-0000-000000000000
        [HttpGet("{routeId}")]

        public async Task<ActionResult<RouteDTO>> Get(Guid routeId)
        {
            var route = await _knownRouteService.GetByIdAsync(routeId);
            if (route == null)
                return NotFound("MapPoint not found.");

            return Ok(new KnownRouteDTO(route));
        }

        [HttpGet("{mapPoint1Id}/{mapPoint2Id}")]
        public async Task<ActionResult<KnownRouteDTO>> CalculateRoute(Guid mapPoint1Id, Guid mapPoint2Id)
        {
            var knownRoute = await _knownRouteService.FindBestRouteAsync(mapPoint1Id, mapPoint2Id);

            return new KnownRouteDTO(knownRoute);
        }
    }
}
