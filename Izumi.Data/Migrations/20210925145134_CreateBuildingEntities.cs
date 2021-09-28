using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class CreateBuildingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "buildings",
                columns: table => new
                {
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_buildings", x => x.type);
                });

            migrationBuilder.CreateTable(
                name: "building_ingredients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    ingredient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    building_type = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_building_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_building_ingredients_buildings_building_type",
                        column: x => x.building_type,
                        principalTable: "buildings",
                        principalColumn: "type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_buildings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    durability = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    building_type = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_buildings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_buildings_buildings_building_type",
                        column: x => x.building_type,
                        principalTable: "buildings",
                        principalColumn: "type",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_buildings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_building_ingredients_building_type_category_ingredient_id",
                table: "building_ingredients",
                columns: new[] { "building_type", "category", "ingredient_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_buildings_type",
                table: "buildings",
                column: "type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_buildings_building_type",
                table: "user_buildings",
                column: "building_type");

            migrationBuilder.CreateIndex(
                name: "ix_user_buildings_user_id_building_type",
                table: "user_buildings",
                columns: new[] { "user_id", "building_type" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "building_ingredients");

            migrationBuilder.DropTable(
                name: "user_buildings");

            migrationBuilder.DropTable(
                name: "buildings");
        }
    }
}
