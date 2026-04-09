using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VibeWave.Migrations
{
    /// <inheritdoc />
    public partial class SeedConcertTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Concert",
                columns: new[] { "Id", "ActorName", "ConcertCategory", "ConcertName", "DisplayDate", "DisplayTime" },
                values: new object[,]
                {
                    { 2, "ABC", "Comedy", "Kyla Cobbler - Not My Lemons", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0) },
                    { 3, "ABC", "Rock and Pop", "Nurse Georgie Carroll - Infectious", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
