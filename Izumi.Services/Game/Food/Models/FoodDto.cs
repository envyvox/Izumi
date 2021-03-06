using System;
using System.Collections.Generic;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Food.Models
{
    public record FoodDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        bool RecipeSellable,
        bool IsSpecial,
        List<FoodIngredientDto> Ingredients);

    public class FoodProfile : Profile
    {
        public FoodProfile() => CreateMap<Data.Entities.Resource.Food, FoodDto>();
    }
}
