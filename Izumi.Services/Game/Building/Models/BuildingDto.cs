using System.Collections.Generic;
using AutoMapper;
using Izumi.Data.Entities;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Building.Models
{
    public record BuildingDto(
        BuildingCategoryType Category,
        BuildingType Type,
        string Name,
        string Description,
        List<BuildingIngredient> Ingredients);

    public class BuildingProfile : Profile
    {
        public BuildingProfile() => CreateMap<Data.Entities.Building, BuildingDto>();
    }
}
