using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VibeWave.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddConcertToDbAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Concert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcertName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConcertCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConcertLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DisplayTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concert", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Concert",
                columns: new[] { "Id", "ActorName", "ConcertCategory", "ConcertLocation", "ConcertName", "DisplayDate", "DisplayTime" },
                values: new object[,]
                {
                    { 1, "ABC", "More Music", "A", "Come Together - Born to Run - Bruce Springsteen", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0) },
                    { 2, "ABC", "Comedy", "A", "Kyla Cobbler - Not My Lemons", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0) },
                    { 3, "ABC", "Rock and Pop", "A", "Nurse Georgie Carroll - Infectious", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Concert");
        }
    }
}
