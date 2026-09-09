using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUREVA.Migrations
{
    /// <inheritdoc />
    public partial class AddClientDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoriteService",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisit",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Clients",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSpent",
                table: "Clients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalVisits",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteService",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LastVisit",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TotalSpent",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TotalVisits",
                table: "Clients");
        }
    }
}
