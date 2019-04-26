using System.ComponentModel.DataAnnotations.Schema;
using System;
using GeoCoordinatePortable;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Entities.Validations;
using Xpto.Domain.Validation;
using Xpto.Domain.Entities.Common;

namespace Xpto.Domain.Entities
{
    public class MapPoint : Entity, ISelfValidation
    {
        public string Name {get;set;}
        public double Latitude {get;set;}
        public double Longitude {get;set;}

        public GeoCoordinate Coordinate { get; set; }

        public ValidationResult ValidationResult { get; private set; }

        public MapPoint()
        {
            Coordinate = new GeoCoordinate();
        }
        public MapPoint(string name, double latitude, double longitude)
        {
            Coordinate = new GeoCoordinate();
            
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public bool IsValid
        {
            get
            {
                var fiscal = new MapPointIsValidValidation();
                ValidationResult = fiscal.Valid(this);
                return ValidationResult.IsValid;
            }
        }
    }
}
