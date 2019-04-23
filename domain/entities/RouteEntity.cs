using System.Collections.Generic;
using GeoCoordinatePortable;

namespace domain.entities
{
    public class RouteEntity : EntityBase
    {
        public MapPointEntity From { get; set; }
        public MapPointEntity To { get; set; }
        public List<MapPointEntity> Points { get; set; }
        public double Cost { get; set; }
    }
}
