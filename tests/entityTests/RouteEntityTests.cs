using System;
using Xunit;
using GeoCoordinatePortable;
using domain.entities;
using domain.dto;

namespace tests.entityTests
{
    public class RouteEntityTests
    {
        private static Guid _from = Guid.NewGuid();
        private static Guid _to = Guid.NewGuid();

        [Fact]
        public void MustCreateNewBlankRoute()
        {
            var route = new RouteEntity();

            Assert.NotNull(route);
        }

        [Fact]
        public void MustCreateNewRouteAndSetProps()
        {
            var route = new RouteEntity();
            route.FromId = _from;
            route.ToId = _to;

            Assert.Equal(_from, route.FromId);
            Assert.Equal(_to, route.ToId);
        }

        [Fact]
        public void MustCreateNewRouteFromValues()
        {
            var route = new RouteEntity(_from, _to);

            Assert.Equal(_from, route.FromId);
            Assert.Equal(_to, route.ToId);
        }

        [Fact]
        public void MustCreateDto()
        {
            var route = new RouteEntity(_from, _to);
            var dto = new RouteDTO(route);

            Assert.Equal(_from, dto.From);
            Assert.Equal(_to, dto.To);
        }

        [Fact]
        public void MustCreateFromDto()
        {
            var dto = new RouteDTO
            {
                From = _from,
                To = _to
            };

            var route = dto.ToEntity();

            Assert.Equal(_from, route.FromId);
            Assert.Equal(_to, route.ToId);
        }
    }
}
