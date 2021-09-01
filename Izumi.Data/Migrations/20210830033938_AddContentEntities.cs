using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class AddContentEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "content_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    channel_id = table.Column<long>(type: "bigint", nullable: false),
                    message_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_content_messages_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "content_votes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vote = table.Column<byte>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_votes", x => x.id);
                    table.ForeignKey(
                        name: "fk_content_votes_content_messages_message_id",
                        column: x => x.message_id,
                        principalTable: "content_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_content_votes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_content_messages_user_id_channel_id_message_id",
                table: "content_messages",
                columns: new[] { "user_id", "channel_id", "message_id" });

            migrationBuilder.CreateIndex(
                name: "ix_content_votes_message_id",
                table: "content_votes",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "ix_content_votes_user_id_message_id_vote",
                table: "content_votes",
                columns: new[] { "user_id", "message_id", "vote" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "content_votes");

            migrationBuilder.DropTable(
                name: "content_messages");
        }
    }
}
