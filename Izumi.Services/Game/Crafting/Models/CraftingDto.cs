using System;
using System.Collections.Generic;
using AutoMapper;
using Izumi.Data.Entities.Resource.Ingredients;
using Izumi.Data.Entities.Resource.Properties;

namespace Izumi.Services.Game.Crafting.Models
{
    public record CraftingDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        List<CraftingProperty> Properties,
        List<CraftingIngredient> Ingredients);

    public class CraftingProfile : Profile
    {
        public CraftingProfile() => CreateMap<Data.Entities.Resource.Crafting, CraftingDto>();
    }
}
