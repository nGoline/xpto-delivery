using System;
using Xunit;
using GeoCoordinatePortable;
using Xpto.Domain.Entities;
using Xpto.Domain.DTO;

namespace Xpto.Tests.EntityTests
{
    public class MapPointTests
    {
        private const string _name = "Test Point 1";
        private const double _latitude = 23.56789012D;
        private const double _longitude = -112.67558488D;

        [Fact]
        public void MustCreateNewBlankMapPoint()
        {
            var mapPoint = new MapPoint();

            Assert.NotNull(mapPoint);
        }

        [Fact]
        public void MustCreateNewMapPointAndSetProps()
        {
            var mapPoint = new MapPoint();
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
            var mapPoint = new MapPoint(_name, _latitude, _longitude);

            Assert.Equal(_name, mapPoint.Name);
            Assert.Equal(_latitude, mapPoint.Latitude);
            Assert.Equal(_longitude, mapPoint.Longitude);
        }

        [Fact]
        public void ShouldNotCreateFromInvalidValues()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
            {
                new MapPoint(_name, -300, _longitude);
            });
            Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
            {
                new MapPoint(_name, -_latitude, 300);
            });
            
            Assert.Throws(typeof(NullReferenceException), () =>
            {
                new MapPoint(string.Empty, -_latitude, _longitude);
            });
            Assert.Throws(typeof(NullReferenceException), () =>
            {
                new MapPoint(" ", -_latitude, _longitude);
            });

            var mapPoint = new MapPoint();
            Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
            {
                mapPoint.Latitude = -300;
            });
            Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
            {
                mapPoint.Longitude = -300;
            });            
            Assert.Throws(typeof(NullReferenceException), () =>
            {
                mapPoint.Name = string.Empty;
            });
            Assert.Throws(typeof(NullReferenceException), () =>
            {
                mapPoint.Name = " ";
            });         
        }

        [Fact]
        public void MustCreateDto()
        {
            var mapPoint = new MapPoint(_name, _latitude, _longitude);
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
