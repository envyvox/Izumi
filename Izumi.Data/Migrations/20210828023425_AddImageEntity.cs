using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class AddImageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_images_type",
                table: "images",
                column: "type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "images");
        }
    }
}
