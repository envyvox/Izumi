using System;
using AutoMapper;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Food.Models
{
    public record FoodIngredientDto(
        IngredientCategoryType Category,
        Guid IngredientId,
        uint Amount);

    public class FoodIngredientProfile : Profile
    {
        public FoodIngredientProfile() => CreateMap<FoodIngredient, FoodIngredientDto>();
    }
}