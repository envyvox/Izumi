using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class UpdateUserTitleEntityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_title_users_user_id",
                table: "user_title");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_title",
                table: "user_title");

            migrationBuilder.RenameTable(
                name: "user_title",
                newName: "user_titles");

            migrationBuilder.RenameIndex(
                name: "ix_user_title_user_id_type",
                table: "user_titles",
                newName: "ix_user_titles_user_id_type");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_titles",
                table: "user_titles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_titles_users_user_id",
                table: "user_titles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_titles_users_user_id",
                table: "user_titles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_titles",
                table: "user_titles");

            migrationBuilder.RenameTable(
                name: "user_titles",
                newName: "user_title");

            migrationBuilder.RenameIndex(
                name: "ix_user_titles_user_id_type",
                table: "user_title",
                newName: "ix_user_title_user_id_type");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_title",
                table: "user_title",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_title_users_user_id",
                table: "user_title",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
