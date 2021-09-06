using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class CreateWorldSettingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "world_settings",
                columns: table => new
                {
                    current_season = table.Column<byte>(type: "smallint", nullable: false),
                    weather_today = table.Column<byte>(type: "smallint", nullable: false),
                    weather_tomorrow = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "world_settings");
        }
    }
}
