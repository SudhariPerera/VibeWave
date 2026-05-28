using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeWave.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MakeMessageOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ContactMessages",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000);

            migrationBuilder.InsertData(
                table: "Concert",
                columns: new[] { "Id", "ActorName", "CategoryId", "ConcertImageUrl", "ConcertLocation", "ConcertName", "DisplayDate", "DisplayTime", "TicketPrice" },
                values: new object[] { 4, "ABC", 3, "", "A", "Nurse Georgie Carroll - Infectious", new DateOnly(2026, 5, 1), new TimeOnly(0, 20, 0), 50m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ContactMessages",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);
        }
    }
}
