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
                columns: new[] { "Id", "ConcertCategory", "ConcertName", "DisplayOrder" },
                values: new object[,]
                {
                    { 1, "More Music", "Come Together - Born to Run - Bruce Springsteen", 1 },
                    { 2, "Comedy", "Kyla Cobbler - Not My Lemons", 2 },
                    { 3, "Rock and Pop", "Nurse Georgie Carroll - Infectious", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 1);

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
