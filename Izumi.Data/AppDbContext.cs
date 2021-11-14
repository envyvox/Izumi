using System;
using Izumi.Data.Converters;
using Izumi.Data.Entities;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Entities.Resource;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.UseEntityTypeConfiguration<AppDbContext>();
            modelBuilder.UseSnakeCaseNamingConvention();
            modelBuilder.UseValueConverterForType<DateTime>(new DateTimeUtcKindConverter());
        }

        public DbSet<ContentMessage> ContentMessages { get; set; }
        public DbSet<ContentVote> ContentVotes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<UserMute> UserMutes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserVoice> UserVoices { get; set; }
        public DbSet<UserWarn> UserWarns { get; set; }

        public DbSet<Alcohol> Alcohols { get; set; }
        public DbSet<AlcoholIngredient> AlcoholIngredients { get; set; }
        public DbSet<AlcoholProperty> AlcoholProperties { get; set; }
        public DbSet<Crafting> Craftings { get; set; }
        public DbSet<CraftingIngredient> CraftingIngredients { get; set; }
        public DbSet<CraftingProperty> CraftingProperties { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<DrinkIngredient> DrinkIngredients { get; set; }
        public DbSet<Fish> Fishes { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodIngredient> FoodIngredients { get; set; }
        public DbSet<Gathering> Gatherings { get; set; }
        public DbSet<GatheringProperty> GatheringProperties { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seafood> Seafoods { get; set; }
        public DbSet<Seed> Seeds { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserAlcohol> UserAlcohols { get; set; }
        public DbSet<UserBanner> UserBanners { get; set; }
        public DbSet<UserBox> UserBoxes { get; set; }
        public DbSet<UserBuilding> UserBuildings { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<UserContract> UserContracts { get; set; }
        public DbSet<UserCooldown> UserCooldowns { get; set; }
        public DbSet<UserCrafting> UserCraftings { get; set; }
        public DbSet<UserCrop> UserCrops { get; set; }
        public DbSet<UserCurrency> UserCurrencies { get; set; }
        public DbSet<UserDrink> UserDrinks { get; set; }
        public DbSet<UserEffect> UserEffects { get; set; }
        public DbSet<UserField> UserFields { get; set; }
        public DbSet<UserFish> UserFishes { get; set; }
        public DbSet<UserFood> UserFoods { get; set; }
        public DbSet<UserGathering> UserGatherings { get; set; }
        public DbSet<UserHangfireJob> UserHangfireJobs { get; set; }
        public DbSet<UserMovement> UserMovements { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }
        public DbSet<UserRecipe> UserRecipes { get; set; }
        public DbSet<UserReferrer> UserReferrers { get; set; }
        public DbSet<UserReputation> UserReputations { get; set; }
        public DbSet<UserSeafood> UserSeafoods { get; set; }
        public DbSet<UserSeed> UserSeeds { get; set; }
        public DbSet<UserStatistic> UserStatistics { get; set; }
        public DbSet<UserTitle> UserTitles { get; set; }
        public DbSet<UserTutorial> UserTutorials { get; set; }

        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingIngredient> BuildingIngredients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<DynamicShopBanner> DynamicShopBanners { get; set; }
        public DbSet<DynamicShopRecipe> DynamicShopRecipes { get; set; }
        public DbSet<Localization> Localizations { get; set; }
        public DbSet<Transit> Transits { get; set; }
        public DbSet<WorldProperty> WorldProperties { get; set; }
        public DbSet<WorldSetting> WorldSettings { get; set; }
    }
}