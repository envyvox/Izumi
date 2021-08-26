using System;
using System.Collections.Generic;
using Izumi.Data.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Izumi.Data.Migrations
{
    public partial class ResourceEntitiesInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alcohols",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alcohols", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "craftings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_craftings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drinks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drinks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fishes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    rarity = table.Column<byte>(type: "smallint", nullable: false),
                    catch_weather = table.Column<byte>(type: "smallint", nullable: false),
                    catch_times_day = table.Column<byte>(type: "smallint", nullable: false),
                    catch_seasons = table.Column<List<SeasonType>>(type: "smallint[]", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fishes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "foods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    recipe_sellable = table.Column<bool>(type: "boolean", nullable: false),
                    is_special = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_foods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gatherings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<byte>(type: "smallint", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gatherings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "seafoods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seafoods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "seeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    season = table.Column<byte>(type: "smallint", nullable: false),
                    growth_days = table.Column<long>(type: "bigint", nullable: false),
                    re_growth_days = table.Column<long>(type: "bigint", nullable: false),
                    is_multiply = table.Column<bool>(type: "boolean", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seeds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alcohol_ingredients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    ingredient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    alcohol_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alcohol_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_alcohol_ingredients_alcohols_alcohol_id",
                        column: x => x.alcohol_id,
                        principalTable: "alcohols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alcohol_properties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    property = table.Column<byte>(type: "smallint", nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false),
                    alcohol_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alcohol_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_alcohol_properties_alcohols_alcohol_id",
                        column: x => x.alcohol_id,
                        principalTable: "alcohols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_alcohols",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    alcohol_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_alcohols", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_alcohols_alcohols_alcohol_id",
                        column: x => x.alcohol_id,
                        principalTable: "alcohols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_alcohols_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crafting_ingredients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    ingredient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    crafting_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crafting_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_crafting_ingredients_craftings_crafting_id",
                        column: x => x.crafting_id,
                        principalTable: "craftings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crafting_properties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    property = table.Column<byte>(type: "smallint", nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false),
                    crafting_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crafting_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_crafting_properties_craftings_crafting_id",
                        column: x => x.crafting_id,
                        principalTable: "craftings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_craftings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    crafting_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_craftings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_craftings_craftings_crafting_id",
                        column: x => x.crafting_id,
                        principalTable: "craftings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_craftings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drink_ingredients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    ingredient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    drink_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drink_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_drink_ingredients_drinks_drink_id",
                        column: x => x.drink_id,
                        principalTable: "drinks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_drinks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    drink_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_drinks", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_drinks_drinks_drink_id",
                        column: x => x.drink_id,
                        principalTable: "drinks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_drinks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_fishes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    fish_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_fishes", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_fishes_fishes_fish_id",
                        column: x => x.fish_id,
                        principalTable: "fishes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_fishes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "food_ingredients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    ingredient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    food_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_food_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_food_ingredients_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_foods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    food_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_foods", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_foods_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_foods_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gathering_properties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    property = table.Column<byte>(type: "smallint", nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false),
                    gathering_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gathering_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_gathering_properties_gatherings_gathering_id",
                        column: x => x.gathering_id,
                        principalTable: "gatherings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_gatherings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    gathering_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_gatherings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_gatherings_gatherings_gathering_id",
                        column: x => x.gathering_id,
                        principalTable: "gatherings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_gatherings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_products_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_seafoods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    seafood_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_seafoods", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_seafoods_seafoods_seafood_id",
                        column: x => x.seafood_id,
                        principalTable: "seafoods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_seafoods_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "crops",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    auto_incremented_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<long>(type: "bigint", nullable: false),
                    seed_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crops", x => x.id);
                    table.ForeignKey(
                        name: "fk_crops_seeds_seed_id",
                        column: x => x.seed_id,
                        principalTable: "seeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_seeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    seed_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_seeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_seeds_seeds_seed_id",
                        column: x => x.seed_id,
                        principalTable: "seeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_seeds_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_crops",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    crop_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_crops", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_crops_crops_crop_id",
                        column: x => x.crop_id,
                        principalTable: "crops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_crops_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_alcohol_ingredients_alcohol_id_category_ingredient_id",
                table: "alcohol_ingredients",
                columns: new[] { "alcohol_id", "category", "ingredient_id" });

            migrationBuilder.CreateIndex(
                name: "ix_alcohol_properties_alcohol_id_property",
                table: "alcohol_properties",
                columns: new[] { "alcohol_id", "property" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_alcohols_name",
                table: "alcohols",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crafting_ingredients_crafting_id_category_ingredient_id",
                table: "crafting_ingredients",
                columns: new[] { "crafting_id", "category", "ingredient_id" });

            migrationBuilder.CreateIndex(
                name: "ix_crafting_properties_crafting_id_property",
                table: "crafting_properties",
                columns: new[] { "crafting_id", "property" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_craftings_name",
                table: "craftings",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crops_name",
                table: "crops",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_crops_seed_id",
                table: "crops",
                column: "seed_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_drink_ingredients_drink_id_category_ingredient_id",
                table: "drink_ingredients",
                columns: new[] { "drink_id", "category", "ingredient_id" });

            migrationBuilder.CreateIndex(
                name: "ix_drinks_name",
                table: "drinks",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_fishes_name",
                table: "fishes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_food_ingredients_food_id_category_ingredient_id",
                table: "food_ingredients",
                columns: new[] { "food_id", "category", "ingredient_id" });

            migrationBuilder.CreateIndex(
                name: "ix_foods_name",
                table: "foods",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gathering_properties_gathering_id_property",
                table: "gathering_properties",
                columns: new[] { "gathering_id", "property" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gatherings_name",
                table: "gatherings",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_name",
                table: "products",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_seafoods_name",
                table: "seafoods",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_seeds_name",
                table: "seeds",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_alcohols_alcohol_id",
                table: "user_alcohols",
                column: "alcohol_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_alcohols_user_id_alcohol_id",
                table: "user_alcohols",
                columns: new[] { "user_id", "alcohol_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_craftings_crafting_id",
                table: "user_craftings",
                column: "crafting_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_craftings_user_id_crafting_id",
                table: "user_craftings",
                columns: new[] { "user_id", "crafting_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_crops_crop_id",
                table: "user_crops",
                column: "crop_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_crops_user_id_crop_id",
                table: "user_crops",
                columns: new[] { "user_id", "crop_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_drinks_drink_id",
                table: "user_drinks",
                column: "drink_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_drinks_user_id_drink_id",
                table: "user_drinks",
                columns: new[] { "user_id", "drink_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_fishes_fish_id",
                table: "user_fishes",
                column: "fish_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_fishes_user_id_fish_id",
                table: "user_fishes",
                columns: new[] { "user_id", "fish_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_foods_food_id",
                table: "user_foods",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_foods_user_id_food_id",
                table: "user_foods",
                columns: new[] { "user_id", "food_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_gatherings_gathering_id",
                table: "user_gatherings",
                column: "gathering_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_gatherings_user_id_gathering_id",
                table: "user_gatherings",
                columns: new[] { "user_id", "gathering_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_products_product_id",
                table: "user_products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_products_user_id_product_id",
                table: "user_products",
                columns: new[] { "user_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_seafoods_seafood_id",
                table: "user_seafoods",
                column: "seafood_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_seafoods_user_id_seafood_id",
                table: "user_seafoods",
                columns: new[] { "user_id", "seafood_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_seeds_seed_id",
                table: "user_seeds",
                column: "seed_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_seeds_user_id_seed_id",
                table: "user_seeds",
                columns: new[] { "user_id", "seed_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alcohol_ingredients");

            migrationBuilder.DropTable(
                name: "alcohol_properties");

            migrationBuilder.DropTable(
                name: "crafting_ingredients");

            migrationBuilder.DropTable(
                name: "crafting_properties");

            migrationBuilder.DropTable(
                name: "drink_ingredients");

            migrationBuilder.DropTable(
                name: "food_ingredients");

            migrationBuilder.DropTable(
                name: "gathering_properties");

            migrationBuilder.DropTable(
                name: "user_alcohols");

            migrationBuilder.DropTable(
                name: "user_craftings");

            migrationBuilder.DropTable(
                name: "user_crops");

            migrationBuilder.DropTable(
                name: "user_drinks");

            migrationBuilder.DropTable(
                name: "user_fishes");

            migrationBuilder.DropTable(
                name: "user_foods");

            migrationBuilder.DropTable(
                name: "user_gatherings");

            migrationBuilder.DropTable(
                name: "user_products");

            migrationBuilder.DropTable(
                name: "user_seafoods");

            migrationBuilder.DropTable(
                name: "user_seeds");

            migrationBuilder.DropTable(
                name: "alcohols");

            migrationBuilder.DropTable(
                name: "craftings");

            migrationBuilder.DropTable(
                name: "crops");

            migrationBuilder.DropTable(
                name: "drinks");

            migrationBuilder.DropTable(
                name: "fishes");

            migrationBuilder.DropTable(
                name: "foods");

            migrationBuilder.DropTable(
                name: "gatherings");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "seafoods");

            migrationBuilder.DropTable(
                name: "seeds");
        }
    }
}
