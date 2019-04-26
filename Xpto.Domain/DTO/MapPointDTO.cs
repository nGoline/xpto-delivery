using System;
using Xpto.Domain.Entities;

namespace Xpto.Domain.DTO
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
        public MapPointDTO(MapPoint entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Latitude = entity.Latitude;
            Longitude = entity.Longitude;
        }

        public MapPoint ToEntity()
        {
            return new MapPoint
            {
                Name = Name,
                Latitude = Latitude,
                Longitude = Longitude,
                Id = Id
            };
        }
    }
}