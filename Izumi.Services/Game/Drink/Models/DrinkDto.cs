using System;
using System.Collections.Generic;
using AutoMapper;
using Izumi.Data.Entities.Resource.Ingredients;

namespace Izumi.Services.Game.Drink.Models
{
    public record DrinkDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        List<DrinkIngredient> Ingredients);

    public class DrinkProfile : Profile
    {
        public DrinkProfile() => CreateMap<Data.Entities.Resource.Drink, DrinkDto>();
    }
}
