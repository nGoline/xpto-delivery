using System;
using Xpto.Domain.Entities;

namespace Xpto.Domain.DTO
{
    [Serializable]
    public class RouteDTO
    {
        public Guid Id { get; set; }
        public MapPointDTO From { get; set; }
        public MapPointDTO To { get; set; }
        public double Cost { get; set; }
        public double Time { get; set; }

        public RouteDTO()
        { }
        public RouteDTO(Route entity)
        {
            Id = entity.Id;
            From = new MapPointDTO { Id = entity.FromId, Latitude = entity.From.Latitude, Longitude = entity.From.Longitude };
            To = new MapPointDTO { Id = entity.ToId, Latitude = entity.To.Latitude, Longitude = entity.To.Longitude };
            Cost = entity.Cost;
            Time = entity.Time;
        }

        public Route ToEntity()
        {
            return new Route
            {
                Id = Id,
                FromId = From.Id,
                From = From.ToEntity(),
                ToId = To.Id,
                To = To.ToEntity(),
                Cost = Cost,
                Time = Time
            };
        }
    }
}