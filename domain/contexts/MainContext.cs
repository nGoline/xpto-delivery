using System;
using domain.entities;
using Microsoft.EntityFrameworkCore;

namespace domain.contexts
{
    public class MainContext : DbContext
    {
        /// <summary>
        /// User DbSet that represents the database entries
        /// </summary>
        /// <value></value>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// MapPoint DbSet that represents the database entries
        /// </summary>
        /// <value></value>
        public DbSet<MapPointEntity> MapPoints { get; set; }

        /// <summary>
        /// Context constructor takes DbContextOptions needed from IoC injection
        /// </summary>
        /// <param name="options"></param>
        public MainContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Instructions to be executed when creating the database
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder object</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pre-populates users to the database
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane"
                },
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "John"
                }
            );

            // User Rules
            modelBuilder.Entity<UserEntity>(b =>
            {
                // Sets required fields
                b.Property(u => u.Name)
                 .IsRequired();
            });

            // MapPointRules
            modelBuilder.Entity<MapPointEntity>(b =>
            {
                // Setup table
                b.ToTable("MapPoints");

                // Sets required fields
                b.Property(mp => mp.Name)
                 .IsRequired();
                b.Property(mp => mp.Latitude)
                 .IsRequired();
                b.Property(mp => mp.Longitude)
                 .IsRequired();
            });
        }
    }
}