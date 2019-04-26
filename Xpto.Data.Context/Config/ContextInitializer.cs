using System.Collections.Generic;
using System.Linq;
using Xpto.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context;

namespace Xpto.Data.Context.Config
{
    public static class ContextInitializer
    {
        public static void Seed(XptoContext context)
        {
            var users = new List<User>
            {
                new User { Name = "Admin", Email = "admin@xpto.com"},
                new User { Name = "Mike", Email = "mike@xpto.com"}
            };
            users.ForEach(u => context.Users.Add(u) );

            var mapPoints = new List<MapPoint>
            {
                new MapPoint { Name = "Test point 1", Latitude = 10, Longitude = 10 },
                new MapPoint { Name = "Test point 2", Latitude = 11, Longitude = 11 },
                new MapPoint { Name = "Test point 3", Latitude = 10, Longitude = 12 },
                new MapPoint { Name = "Test point 4", Latitude =11, Longitude = -5 }
            };
            mapPoints.ForEach(mp => context.MapPoints.Add(mp));

            context.SaveChanges();
        }
    }
}
