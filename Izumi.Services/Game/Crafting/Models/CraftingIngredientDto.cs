using System;
using AutoMapper;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Crafting.Models
{
    public record CraftingIngredientDto(
        IngredientCategoryType Category,
        Guid IngredientId,
        uint Amount);

    public class CraftingIngredientProfile : Profile
    {
        public CraftingIngredientProfile() => CreateMap<CraftingIngredient, CraftingIngredientDto>();
    }
}