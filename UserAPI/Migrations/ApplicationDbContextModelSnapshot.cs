﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserAPI.Data;

#nullable disable

namespace UserAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserAPI.Models.LocalUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LocalUsers");
                });

            modelBuilder.Entity("UserAPI.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Kcal")
                        .HasColumnType("int");

                    b.Property<double>("Mass")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfProducts")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("05cbf9f0-959e-465a-8059-48a0f232d5c1"),
                            CreatedDate = new DateTime(2024, 5, 3, 19, 7, 40, 588, DateTimeKind.Local).AddTicks(647),
                            Description = "Description",
                            Kcal = 10,
                            Mass = 0.0,
                            Name = "Курячі яйця",
                            NumberOfProducts = 0
                        },
                        new
                        {
                            Id = new Guid("a3efdc48-6efc-4fcf-97d9-00659f8b90df"),
                            CreatedDate = new DateTime(2024, 5, 3, 19, 7, 40, 588, DateTimeKind.Local).AddTicks(718),
                            Description = "Description",
                            Kcal = 15,
                            Mass = 0.0,
                            Name = "Картопля",
                            NumberOfProducts = 0
                        },
                        new
                        {
                            Id = new Guid("74e92459-af85-4c35-8be1-f3d8a5cbc50b"),
                            CreatedDate = new DateTime(2024, 5, 3, 19, 7, 40, 588, DateTimeKind.Local).AddTicks(904),
                            Description = "Description",
                            Kcal = 25,
                            Mass = 0.0,
                            Name = "Цибуля",
                            NumberOfProducts = 0
                        });
                });

            modelBuilder.Entity("UserAPI.Models.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Recipes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("95120bde-4548-4a4b-9faf-89f03f55553b"),
                            Description = "Description",
                            Name = "Яєчня",
                            ProductIds = "[\"05cbf9f0-959e-465a-8059-48a0f232d5c1\"]"
                        },
                        new
                        {
                            Id = new Guid("0e15f14a-174d-407c-8d0d-1163c01ac213"),
                            Description = "Description",
                            Name = "Картопля з цибулею",
                            ProductIds = "[\"a3efdc48-6efc-4fcf-97d9-00659f8b90df\",\"74e92459-af85-4c35-8be1-f3d8a5cbc50b\"]"
                        },
                        new
                        {
                            Id = new Guid("456ad79d-2561-4039-a83a-4c98d7bfffaf"),
                            Description = "Description",
                            Name = "Картопля з яйцем та цибулею",
                            ProductIds = "[\"05cbf9f0-959e-465a-8059-48a0f232d5c1\",\"a3efdc48-6efc-4fcf-97d9-00659f8b90df\",\"74e92459-af85-4c35-8be1-f3d8a5cbc50b\"]"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
