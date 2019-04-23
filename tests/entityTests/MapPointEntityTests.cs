using System;
using Xunit;
using GeoCoordinatePortable;
using domain.entities;
using domain.dto;

namespace tests.entityTests
{
    public class MapPointEntityTests
    {
        private const string _name = "Test Point 1";
        private const double _latitude = 23.56789012D;
        private const double _longitude = -112.67558488D;

        [Fact]
        public void MustCreateNewBlankMapPoint()
        {
            var mapPoint = new MapPointEntity();

            Assert.NotNull(mapPoint);
        }

        [Fact]
        public void MustCreateNewMapPointAndSetProps()
        {
            var mapPoint = new MapPointEntity();
            mapPoint.Name = _name;
            mapPoint.Latitude = _latitude;
            mapPoint.Longitude = _longitude;

            Assert.Equal(_name, mapPoint.Name);
            Assert.Equal(_latitude, mapPoint.Latitude);
            Assert.Equal(_longitude, mapPoint.Longitude);
        }

        [Fact]
        public void MustCreateNewMapPointFromValues()
        {
            var mapPoint = new MapPointEntity(_name, _latitude, _longitude);

            Assert.Equal(_name, mapPoint.Name);
            Assert.Equal(_latitude, mapPoint.Latitude);
            Assert.Equal(_longitude, mapPoint.Longitude);
        }

        [Fact]
        public void MustCreateDto()
        {
            var mapPoint = new MapPointEntity(_name, _latitude, _longitude);
            var dto = new MapPointDTO(mapPoint);

            Assert.Equal(_name, dto.Name);
            Assert.Equal(_latitude, dto.Latitude);
            Assert.Equal(_longitude, dto.Longitude);
        }

        [Fact]
        public void MustCreateFromDto()
        {
            var dto = new MapPointDTO
            {
                Name = _name,
                Latitude = _latitude,
                Longitude = _longitude
            };

            var mapPoint = dto.ToEntity();

            Assert.Equal(_name, mapPoint.Name);
            Assert.Equal(_latitude, mapPoint.Latitude);
            Assert.Equal(_longitude, mapPoint.Longitude);
        }
    }
}
