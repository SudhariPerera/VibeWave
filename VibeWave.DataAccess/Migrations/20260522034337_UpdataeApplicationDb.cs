using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeWave.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdataeApplicationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 1,
                column: "DisplayDate",
                value: new DateOnly(2026, 6, 10));

            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 2,
                column: "DisplayDate",
                value: new DateOnly(2026, 6, 12));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 1,
                column: "DisplayDate",
                value: new DateOnly(2026, 5, 1));

            migrationBuilder.UpdateData(
                table: "Concert",
                keyColumn: "Id",
                keyValue: 2,
                column: "DisplayDate",
                value: new DateOnly(2026, 5, 1));
        }
    }
}
