using System;
using AutoMapper;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Drink.Models
{
    public record DrinkIngredientDto(
        IngredientCategoryType Category,
        Guid IngredientId,
        uint Amount);

    public class DrinkIngredientProfile : Profile
    {
        public DrinkIngredientProfile() => CreateMap<DrinkIngredient, DrinkIngredientDto>();
    }
}