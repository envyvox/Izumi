using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Izumi.Data.Migrations
{
    public partial class CreateUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    about = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<byte>(type: "smallint", nullable: false),
                    gender = table.Column<byte>(type: "smallint", nullable: false),
                    location = table.Column<byte>(type: "smallint", nullable: false),
                    energy = table.Column<long>(type: "bigint", nullable: false),
                    points = table.Column<long>(type: "bigint", nullable: false),
                    is_premium = table.Column<bool>(type: "boolean", nullable: false),
                    command_color = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_id",
                table: "users",
                column: "id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
