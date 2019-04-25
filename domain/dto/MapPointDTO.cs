using System;
using domain.entities;

namespace domain.dto
{
    [Serializable]
    public class MapPointDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public MapPointDTO()
        { }
        public MapPointDTO(MapPointEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Latitude = entity.Latitude;
            Longitude = entity.Longitude;
        }

        public MapPointEntity ToEntity()
        {
            return new MapPointEntity(Name, Latitude, Longitude) { Id = Id };
        }
    }
}