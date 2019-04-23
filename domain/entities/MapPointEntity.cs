using GeoCoordinatePortable;

namespace domain.entities
{
    public class MapPointEntity : EntityBase
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public MapPointEntity()
        {}
        public MapPointEntity(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
