using System;
using Izumi.Data;
using Izumi.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Localization.Impl
{
    public class LocalizationService : ILocalizationService
    {
        private readonly AppDbContext _db;

        public LocalizationService(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public string Localize(LocalizationCategoryType category, string keyword, uint amount = 1)
        {
            var entity = _db.Localizations
                .SingleOrDefaultAsync(x =>
                    x.Category == category &&
                    x.Name == keyword)
                .Result;

            if (entity is null)
            {
                throw new Exception(
                    $"localization with category {category.ToString()} and keyword {keyword} not found");
            }

            return entity.Localize(amount);
        }

        public string Localize(IngredientCategoryType category, string keyword, uint amount = 1)
        {
            return Localize(category switch
            {
                IngredientCategoryType.Gathering => LocalizationCategoryType.Gathering,
                IngredientCategoryType.Product => LocalizationCategoryType.Product,
                IngredientCategoryType.Crafting => LocalizationCategoryType.Crafting,
                IngredientCategoryType.Alcohol => LocalizationCategoryType.Alcohol,
                IngredientCategoryType.Drink => LocalizationCategoryType.Drink,
                IngredientCategoryType.Crop => LocalizationCategoryType.Crop,
                IngredientCategoryType.Food => LocalizationCategoryType.Food,
                IngredientCategoryType.Seafood => LocalizationCategoryType.Seafood,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, keyword, amount);
        }
    }
}