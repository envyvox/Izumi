using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class CreateLocalizationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "localization",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    single = table.Column<string>(type: "text", nullable: false),
                    @double = table.Column<string>(name: "double", type: "text", nullable: false),
                    multiply = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localization", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_localization_category_name",
                table: "localization",
                columns: new[] { "category", "name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "localization");
        }
    }
}
