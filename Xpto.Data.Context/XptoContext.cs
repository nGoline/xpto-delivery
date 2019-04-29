using System;
using Microsoft.EntityFrameworkCore;

using Xpto.Data.Context.Mapping;
using Xpto.Domain.Entities;

namespace Xpto.Data.Context
{
    public class XptoContext : DbContext
    {
        public XptoContext(DbContextOptions options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new MapPointMap());
            modelBuilder.ApplyConfiguration(new RouteMap());

            modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid(), Name = "Admin", Email = "admin@xpto.com", Password = "123456" },
                new User { Id = Guid.NewGuid(), Name = "Mike", Email = "mike@xpto.com", Password = "123456" }
            );

            var mapPoints = new MapPoint[]
            {
                new MapPoint { Id = Guid.NewGuid(), Name = "A", Latitude = 10, Longitude = 10 },
                new MapPoint { Id = Guid.NewGuid(), Name = "B", Latitude = 11, Longitude = 11 },
                new MapPoint { Id = Guid.NewGuid(), Name = "C", Latitude = 10, Longitude = 12 },
                new MapPoint { Id = Guid.NewGuid(), Name = "D", Latitude = 11, Longitude = -5 },
                new MapPoint { Id = Guid.NewGuid(), Name = "E", Latitude = 9, Longitude = -5 },
                new MapPoint { Id = Guid.NewGuid(), Name = "F", Latitude = 13, Longitude = 8 },
                new MapPoint { Id = Guid.NewGuid(), Name = "G", Latitude = 9, Longitude = -2 },
                new MapPoint { Id = Guid.NewGuid(), Name = "H", Latitude = 13, Longitude = 5 },
                new MapPoint { Id = Guid.NewGuid(), Name = "I", Latitude = 1, Longitude = 20 }
            };

            modelBuilder.Entity<MapPoint>().HasData(mapPoints);

            modelBuilder.Entity<Route>().HasData(
                new Route(mapPoints[0].Id, mapPoints[2].Id, 1, 20) { Id = Guid.NewGuid() },
                new Route(mapPoints[0].Id, mapPoints[4].Id, 30, 5) { Id = Guid.NewGuid() },
                new Route(mapPoints[0].Id, mapPoints[7].Id, 10, 1) { Id = Guid.NewGuid() },
                new Route(mapPoints[2].Id, mapPoints[1].Id, 1, 12) { Id = Guid.NewGuid() },
                new Route(mapPoints[3].Id, mapPoints[5].Id, 4, 50) { Id = Guid.NewGuid() },
                new Route(mapPoints[4].Id, mapPoints[3].Id, 3, 5) { Id = Guid.NewGuid() },
                new Route(mapPoints[5].Id, mapPoints[6].Id, 40, 50) { Id = Guid.NewGuid() },
                new Route(mapPoints[5].Id, mapPoints[8].Id, 45, 50) { Id = Guid.NewGuid() },
                new Route(mapPoints[6].Id, mapPoints[1].Id, 64, 73) { Id = Guid.NewGuid() },
                new Route(mapPoints[8].Id, mapPoints[1].Id, 65, 5) { Id = Guid.NewGuid() }
            );
        }

#region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<MapPoint> MapPoints { get; set; }
        public DbSet<Route> Routes { get; set; }
#endregion
    }
}
