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
        private string _name;
        private double _latitude;
        private double _longitude;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException(nameof(Name));

                _name = value;
            }
        }
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
                Coordinate.Latitude = value;
            }
        }
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                Coordinate.Longitude = value;
            }
        }

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
