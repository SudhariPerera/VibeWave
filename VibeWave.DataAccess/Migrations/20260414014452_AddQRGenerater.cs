using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeWave.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddQRGenerater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodeUrl",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCodeUrl",
                table: "Bookings");
        }
    }
}
