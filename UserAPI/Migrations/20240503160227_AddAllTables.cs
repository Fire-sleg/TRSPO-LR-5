﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kcal = table.Column<int>(type: "int", nullable: false),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    NumberOfProducts = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "Description", "Kcal", "Mass", "Name", "NumberOfProducts", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0463431c-2f71-4ffb-a908-d6b5044b74e2"), new DateTime(2024, 5, 3, 19, 2, 26, 834, DateTimeKind.Local).AddTicks(9545), "Description", 15, 0.0, "Картопля", 0, null },
                    { new Guid("35ec7399-15aa-4b88-88e2-418fe38209dc"), new DateTime(2024, 5, 3, 19, 2, 26, 834, DateTimeKind.Local).AddTicks(9490), "Description", 10, 0.0, "Курячі яйця", 0, null },
                    { new Guid("b1404200-c96e-4d77-9d8a-c5a3c6eb8063"), new DateTime(2024, 5, 3, 19, 2, 26, 834, DateTimeKind.Local).AddTicks(9576), "Description", 25, 0.0, "Цибуля", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CreatedDate", "Description", "ImageURL", "Name", "ProductIds", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0e9d3a5c-71bc-4847-bca7-ab0b1f9a0b85"), null, "Description", null, "Картопля з яйцем та цибулею", "[\"35ec7399-15aa-4b88-88e2-418fe38209dc\",\"0463431c-2f71-4ffb-a908-d6b5044b74e2\",\"b1404200-c96e-4d77-9d8a-c5a3c6eb8063\"]", null },
                    { new Guid("75b974d1-e2be-4714-b9c2-8e5fd3dd1fc9"), null, "Description", null, "Картопля з цибулею", "[\"0463431c-2f71-4ffb-a908-d6b5044b74e2\",\"b1404200-c96e-4d77-9d8a-c5a3c6eb8063\"]", null },
                    { new Guid("e9c679aa-cb3d-44ce-bb7a-3c88abbef149"), null, "Description", null, "Яєчня", "[\"35ec7399-15aa-4b88-88e2-418fe38209dc\"]", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
