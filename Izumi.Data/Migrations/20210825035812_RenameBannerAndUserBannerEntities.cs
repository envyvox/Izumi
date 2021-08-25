using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class RenameBannerAndUserBannerEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_banner_banner_banner_id",
                table: "user_banner");

            migrationBuilder.DropForeignKey(
                name: "fk_user_banner_users_user_id",
                table: "user_banner");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_banner",
                table: "user_banner");

            migrationBuilder.DropPrimaryKey(
                name: "pk_banner",
                table: "banner");

            migrationBuilder.RenameTable(
                name: "user_banner",
                newName: "user_banners");

            migrationBuilder.RenameTable(
                name: "banner",
                newName: "banners");

            migrationBuilder.RenameIndex(
                name: "ix_user_banner_user_id_banner_id_is_active",
                table: "user_banners",
                newName: "ix_user_banners_user_id_banner_id_is_active");

            migrationBuilder.RenameIndex(
                name: "ix_user_banner_banner_id",
                table: "user_banners",
                newName: "ix_user_banners_banner_id");

            migrationBuilder.RenameIndex(
                name: "ix_banner_name",
                table: "banners",
                newName: "ix_banners_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_banners",
                table: "user_banners",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_banners",
                table: "banners",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_banners_banners_banner_id",
                table: "user_banners",
                column: "banner_id",
                principalTable: "banners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_banners_users_user_id",
                table: "user_banners",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_banners_banners_banner_id",
                table: "user_banners");

            migrationBuilder.DropForeignKey(
                name: "fk_user_banners_users_user_id",
                table: "user_banners");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_banners",
                table: "user_banners");

            migrationBuilder.DropPrimaryKey(
                name: "pk_banners",
                table: "banners");

            migrationBuilder.RenameTable(
                name: "user_banners",
                newName: "user_banner");

            migrationBuilder.RenameTable(
                name: "banners",
                newName: "banner");

            migrationBuilder.RenameIndex(
                name: "ix_user_banners_user_id_banner_id_is_active",
                table: "user_banner",
                newName: "ix_user_banner_user_id_banner_id_is_active");

            migrationBuilder.RenameIndex(
                name: "ix_user_banners_banner_id",
                table: "user_banner",
                newName: "ix_user_banner_banner_id");

            migrationBuilder.RenameIndex(
                name: "ix_banners_name",
                table: "banner",
                newName: "ix_banner_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_banner",
                table: "user_banner",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_banner",
                table: "banner",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_banner_banner_banner_id",
                table: "user_banner",
                column: "banner_id",
                principalTable: "banner",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_banner_users_user_id",
                table: "user_banner",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
