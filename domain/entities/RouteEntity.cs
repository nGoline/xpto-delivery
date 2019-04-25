using System;
using System.Collections.Generic;
using GeoCoordinatePortable;

namespace domain.entities
{
    public class RouteEntity : EntityBase
    {
        public Guid FromId { get; set; }
        public virtual MapPointEntity From { get; set; }
        public Guid ToId { get; set; }
        public virtual MapPointEntity To { get; set; }
        public virtual List<MapPointEntity> Points { get; set; }
        public double Cost { get; set; }

        public RouteEntity()
        { }
        public RouteEntity(Guid from, Guid to)
        {
            FromId = from;
            ToId = to;
        }
        public RouteEntity(MapPointEntity from, MapPointEntity to)
        {            
            FromId = from.Id;
            From = from;
            ToId = to.Id;
            To = to;
        }
    }
}
