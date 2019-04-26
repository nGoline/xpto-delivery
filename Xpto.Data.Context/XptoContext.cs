using Microsoft.EntityFrameworkCore;
using Xpto.Data.Context.Config;
using Xpto.Data.Context.Mapping;
using Xpto.Domain.Entities;

namespace Xpto.Data.Context
{
    public class XptoContext : DbContext
    {
        public XptoContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new MapPointMap());
            modelBuilder.ApplyConfiguration(new RouteMap());
        }

        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<MapPoint> MapPoints { get; set; }
        public DbSet<Route> Routes { get; set; }
        #endregion
    }
}