using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VibeWave.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTable : Migration
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
                    DisplayTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    TicketPrice = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concert", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcertId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfTickets = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Concert_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "Concert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Concert",
                columns: new[] { "Id", "ActorName", "ConcertCategory", "ConcertLocation", "ConcertName", "DisplayDate", "DisplayTime", "TicketPrice" },
                values: new object[,]
                {
                    { 1, "ABC", "More Music", "A", "Come Together - Born to Run - Bruce Springsteen", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0), "50" },
                    { 2, "ABC", "Comedy", "A", "Kyla Cobbler - Not My Lemons", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0), "50" },
                    { 3, "ABC", "Rock and Pop", "A", "Nurse Georgie Carroll - Infectious", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0), "50" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ConcertId",
                table: "Bookings",
                column: "ConcertId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Concert");
        }
    }
}
