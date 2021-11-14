using System;
using System.Linq;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Hangfire.BackgroundJobs.GenerateDynamicShopRecipe
{
    public class GenerateDynamicShopRecipeJob : IGenerateDynamicShopRecipeJob
    {
        private readonly ILogger<GenerateDynamicShopRecipeJob> _logger;
        private readonly AppDbContext _db;

        public GenerateDynamicShopRecipeJob(
            DbContextOptions options,
            ILogger<GenerateDynamicShopRecipeJob> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task Execute()
        {
            _logger.LogInformation(
                "Generate dynamic shop recipes job executed");

            await _db.Database.ExecuteSqlRawAsync("truncate dynamic_shop_recipes;");

            var foods = await _db.Foods
                .OrderByRandom()
                .Where(x => x.RecipeSellable)
                .ToListAsync();

            foreach (var category in Enum.GetValues(typeof(FoodCategoryType)).Cast<FoodCategoryType>())
            {
                var randomCategoryFood = foods.First(x => x.Category == category);

                await _db.CreateEntity(new DynamicShopRecipe
                {
                    Id = Guid.NewGuid(),
                    FoodId = randomCategoryFood.Id
                });

                _logger.LogInformation(
                    "Created dynamic shop entity for recipe {FoodId}",
                    randomCategoryFood.Id);
            }
        }
    }
}