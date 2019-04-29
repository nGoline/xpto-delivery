using System;
using System.Collections.Generic;

using GeoCoordinatePortable;

using Xpto.Domain.Entities.Common;
using Xpto.Domain.Entities.Validations;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Entities
{
    public class KnownRoute : Entity, ISelfValidation
    {
        public List<Guid> MapPointIds { get; set; }
        public virtual List<MapPoint> MapPoints { get; set; }
        public double Time { get; set; }
        public double Cost { get; set; }

        public ValidationResult ValidationResult { get; private set; }

        public KnownRoute() { }
        public KnownRoute(List<MapPoint> mapPoints, double time, double cost)
        {
            MapPoints = mapPoints;
            MapPointIds = new List<Guid>(mapPoints.Count);
            foreach (var mapPoint in MapPoints)
                MapPointIds.Add(mapPoint.Id);

            Cost = cost;
            Time = time;
        }
        public KnownRoute(List<Guid> mapPointIds, double time, double cost)
        {
            MapPointIds = mapPointIds;
            Cost = cost;
            Time = time;
        }

        public bool IsValid
        {
            get
            {
                var fiscal = new KnownRouteIsValidValidation();
                ValidationResult = fiscal.Valid(this);
                return ValidationResult.IsValid;
            }
        }
    }
}
