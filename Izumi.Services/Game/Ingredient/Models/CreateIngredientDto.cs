using Izumi.Data.Enums;

namespace Izumi.Services.Game.Ingredient.Models
{
    public record CreateIngredientDto(
        IngredientCategoryType Category,
        string Name,
        uint Amount);
}
