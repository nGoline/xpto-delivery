using System;
using System.Collections.Generic;

using Xpto.Domain.Entities;

namespace Xpto.Domain.DTO
{
    public class KnownRouteDTO
    {
        public Guid Id { get; set; }
        public List<MapPointDTO> MapPoints { get; set; }
        public double Time { get; set; }
        public double Cost { get; set; }

        public KnownRouteDTO() { }
        public KnownRouteDTO(KnownRoute entity)
        {
            Id = entity.Id;
            Cost = entity.Cost;
            Time = entity.Time;
            MapPoints = new List<MapPointDTO>();

            foreach (var mapPoint in entity.MapPoints)
                MapPoints.Add(new MapPointDTO(mapPoint));
        }

        public KnownRoute ToEntity()
        {
            var mapPointIds = new List<Guid>();
            foreach (var mapPoint in MapPoints)
                mapPointIds.Add(mapPoint.Id);

            return new KnownRoute
            {
                Id = Id,
                Cost = Cost,
                Time = Time,
                MapPointIds = mapPointIds
            };
        }
    }
}
