using Izumi.Data.Enums;

namespace Izumi.Services.Game.Localization
{
    public interface ILocalizationService
    {
        string Localize(LocalizationCategoryType category, string keyword, uint amount = 1);
        string Localize(IngredientCategoryType category, string keyword, uint amount = 1);
    }
}
