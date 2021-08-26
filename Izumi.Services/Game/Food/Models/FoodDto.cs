using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Food.Models
{
    public record FoodDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        FoodCategoryType Category,
        bool RecipeSellable,
        bool IsSpecial);

    public class FoodProfile : Profile
    {
        public FoodProfile() => CreateMap<Data.Entities.Resource.Food, FoodDto>();
    }
}
