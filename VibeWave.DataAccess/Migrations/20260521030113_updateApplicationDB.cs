using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeWave.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateApplicationDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActorName", "ConcertName" },
                values: new object[] { "Bruce Springsteen", "Come Together - Born to Run" });

            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ActorName", "ConcertName" },
                values: new object[] { "Kyla Cobbler", "Not My Lemons" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActorName", "ConcertName" },
                values: new object[] { "ABC", "Come Together - Born to Run - Bruce Springsteen" });

            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ActorName", "ConcertName" },
                values: new object[] { "ABC", "Kyla Cobbler - Not My Lemons" });
        }
    }
}
