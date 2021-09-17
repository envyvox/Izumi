using System;
using AutoMapper;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Alcohol.Models
{
    public record AlcoholIngredientDto(
        Data.Entities.Resource.Alcohol Alcohol,
        IngredientCategoryType Category,
        Guid IngredientId,
        uint Amount);

    public class AlcoholIngredientProfile : Profile
    {
        public AlcoholIngredientProfile() => CreateMap<AlcoholIngredient, AlcoholIngredientDto>();
    }
}
