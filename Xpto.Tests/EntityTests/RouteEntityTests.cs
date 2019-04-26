using System;
using Xunit;
using GeoCoordinatePortable;
using Xpto.Domain.Entities;
using Xpto.Domain.DTO;

namespace Xpto.Tests.EntityTests
{
    public class RouteTests
    {
        private static MapPoint _from = new MapPoint{Id = Guid.NewGuid(), Name="Test 1", Latitude = 34.12345678, Longitude = -12.12345678 };
        private static MapPoint _to = new MapPoint{Id = Guid.NewGuid(), Name="Test 2", Latitude = 34.12302223, Longitude = -12.00004343 };

        [Fact]
        public void MustCreateNewBlankRoute()
        {
            var route = new Route();

            Assert.NotNull(route);
        }

        [Fact]
        public void MustCreateNewRouteAndSetProps()
        {
            var route = new Route();
            route.From = _from;
            route.FromId = _from.Id;
            route.To = _to;

            Assert.Equal(_from, route.From);
            Assert.Equal(_to, route.To);
        }

        [Fact]
        public void MustCreateNewRouteFromValues()
        {
            var route = new Route(_from, _to);

            Assert.Equal(_from.Id, route.FromId);
            Assert.Equal(_to.Id, route.ToId);
        }

        [Fact]
        public void MustCreateDto()
        {
            var route = new Route(_from, _to);
            var dto = new RouteDTO(route);

            Assert.Equal(_from.Id, dto.From.Id);
            Assert.Equal(_to.Id, dto.To.Id);
        }

        [Fact]
        public void MustCreateFromDto()
        {
            var dto = new RouteDTO
            {
                From = new MapPointDTO(_from),
                To = new MapPointDTO(_to)
            };

            var route = dto.ToEntity();

            Assert.Equal(_from.Id, route.FromId);
            Assert.Equal(_to.Id, route.ToId);
        }
    }
}
