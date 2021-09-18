using System;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Ingredient.Models
{
    public record IngredientDto(
        IngredientCategoryType Category,
        Guid Id,
        string Name
    );
}
