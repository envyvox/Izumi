using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class CreateTransitEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transits",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    departure = table.Column<byte>(type: "smallint", nullable: false),
                    destination = table.Column<byte>(type: "smallint", nullable: false),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transits", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_transits_departure_destination",
                table: "transits",
                columns: new[] { "departure", "destination" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transits");
        }
    }
}
