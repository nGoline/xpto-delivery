using System;
using System.Collections.Generic;
using GeoCoordinatePortable;
using Xpto.Domain.Entities.Validations;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Validation;
using Xpto.Domain.Entities.Common;

namespace Xpto.Domain.Entities
{
    public class Route : Entity, ISelfValidation
    {
        public Guid FromId { get; set; }
        public virtual MapPoint From { get; set; }
        public Guid ToId { get; set; }
        public virtual MapPoint To { get; set; }
        public double Cost { get; set; }
        public double Time { get; set; }

        public ValidationResult ValidationResult { get; private set; }

        public Route()
        { }
        public Route(Guid fromId, Guid toId, double time, double cost)
        {
            FromId = fromId;
            ToId = toId;
            Cost = cost;
            Time = time;
        }
        public Route(MapPoint from, MapPoint to, double time, double cost)
        {
            FromId = from.Id;
            From = from;
            ToId = to.Id;
            To = to;
            Cost = cost;
            Time = time;
        }

        public bool IsValid
        {
            get
            {
                var fiscal = new RouteIsValidValidation();
                ValidationResult = fiscal.Valid(this);
                return ValidationResult.IsValid;
            }
        }
    }
}
