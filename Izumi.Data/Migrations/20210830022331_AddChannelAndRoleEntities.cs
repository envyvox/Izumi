using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class AddChannelAndRoleEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_channels_type",
                table: "channels",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_type",
                table: "roles",
                column: "type",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "channels");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
