using System;
using AutoMapper;

namespace Izumi.Services.Game.Drink.Models
{
    public record DrinkDto(
        Guid Id,
        long AutoIncrementedId,
        string Name);

    public class DrinkProfile : Profile
    {
        public DrinkProfile() => CreateMap<Data.Entities.Resource.Drink, DrinkDto>();
    }
}
