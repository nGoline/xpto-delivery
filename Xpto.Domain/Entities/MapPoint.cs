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
        private GeoCoordinate _coordinate;

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoCoordinate Coordinate
        {
            get
            {
                if (_coordinate == null)
                    _coordinate = new GeoCoordinate(Latitude, Longitude);

                return _coordinate;
            }
            private set
            {
                _coordinate = value;
            }
        }

        public ValidationResult ValidationResult { get; private set; }

        public MapPoint()
        { }
        public MapPoint(string name, double latitude, double longitude)
        {
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
