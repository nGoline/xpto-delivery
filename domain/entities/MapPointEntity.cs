using System.ComponentModel.DataAnnotations.Schema;
using System;
using GeoCoordinatePortable;

namespace domain.entities
{
    public class MapPointEntity : EntityBase
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

        [NotMappedAttribute]
        public GeoCoordinate Coordinate { get; set; }

        public MapPointEntity()
        {
            Coordinate = new GeoCoordinate();
        }
        public MapPointEntity(string name, double latitude, double longitude)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NullReferenceException(nameof(name));

            Coordinate = new GeoCoordinate();
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Validate a DepositAddress object
        /// </summary>
        /// <param name="isNew">True if object should not be found in the Database</param>
        public override void Validate(bool isNew = true)
        {
            base.Validate(isNew);

            // The object should have a Name
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name));
        }
    }
}
