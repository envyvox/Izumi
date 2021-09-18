using System;
using System.Collections.Generic;
using AutoMapper;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Entities.Resource.Properties;

namespace Izumi.Services.Game.Alcohol.Models
{
    public record AlcoholDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        List<AlcoholProperty> Properties,
        List<AlcoholIngredient> Ingredients);

    public class AlcoholProfile : Profile
    {
        public AlcoholProfile() => CreateMap<Data.Entities.Resource.Alcohol, AlcoholDto>();
    }
}
