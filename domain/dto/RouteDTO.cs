using System;
using domain.entities;

namespace domain.dto
{
    public class RouteDTO
    {
        public Guid Id { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }

        public RouteDTO()
        { }
        public RouteDTO(RouteEntity entity)
        {
            Id = entity.Id;
            From = entity.FromId;
            To = entity.ToId;
        }

        public RouteEntity ToEntity()
        {
            return new RouteEntity(From, To) { Id = Id };
        }
    }
}