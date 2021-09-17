using AutoMapper;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Crafting.Models
{
    public record CraftingPropertyDto(
        Data.Entities.Resource.Crafting Crafting,
        CraftingPropertyType Property,
        uint Value);

    public class CraftingPropertyProfile : Profile
    {
        public CraftingPropertyProfile() => CreateMap<CraftingProperty, CraftingPropertyDto>();
    }
}
