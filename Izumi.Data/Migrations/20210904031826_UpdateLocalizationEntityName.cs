using Microsoft.EntityFrameworkCore.Migrations;

namespace Izumi.Data.Migrations
{
    public partial class UpdateLocalizationEntityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_localization",
                table: "localization");

            migrationBuilder.RenameTable(
                name: "localization",
                newName: "localizations");

            migrationBuilder.RenameIndex(
                name: "ix_localization_category_name",
                table: "localizations",
                newName: "ix_localizations_category_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_localizations",
                table: "localizations",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_localizations",
                table: "localizations");

            migrationBuilder.RenameTable(
                name: "localizations",
                newName: "localization");

            migrationBuilder.RenameIndex(
                name: "ix_localizations_category_name",
                table: "localization",
                newName: "ix_localization_category_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_localization",
                table: "localization",
                column: "id");
        }
    }
}
