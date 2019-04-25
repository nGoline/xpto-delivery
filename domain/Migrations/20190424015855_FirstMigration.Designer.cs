﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using domain.contexts;

namespace domain.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20190424015855_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("domain.entities.MapPointEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("RouteEntityId");

                    b.HasKey("Id");

                    b.HasIndex("RouteEntityId");

                    b.ToTable("MapPoints");
                });

            modelBuilder.Entity("domain.entities.RouteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Cost");

                    b.Property<Guid>("FromId");

                    b.Property<Guid>("ToId");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("domain.entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9288af0b-d6c7-4e72-a31d-092aff99a27a"),
                            Name = "Jane"
                        },
                        new
                        {
                            Id = new Guid("5bb83526-75f6-49f6-87b0-46d032ed7c5c"),
                            Name = "John"
                        });
                });

            modelBuilder.Entity("domain.entities.MapPointEntity", b =>
                {
                    b.HasOne("domain.entities.RouteEntity")
                        .WithMany("Points")
                        .HasForeignKey("RouteEntityId");
                });

            modelBuilder.Entity("domain.entities.RouteEntity", b =>
                {
                    b.HasOne("domain.entities.MapPointEntity", "From")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("domain.entities.MapPointEntity", "To")
                        .WithMany()
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}