using System;
using Xpto.Domain.Entities;

namespace Xpto.Domain.DTO
{
    [Serializable]
    public class RouteDTO
    {
        public Guid Id { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }

        public RouteDTO()
        { }
        public RouteDTO(Route entity)
        {
            Id = entity.Id;
            From = entity.FromId;
            To = entity.ToId;
        }

        public Route ToEntity()
        {
            return new Route
            {
                FromId = From,
                ToId = To,
                Id = Id
            };
        }
    }
}