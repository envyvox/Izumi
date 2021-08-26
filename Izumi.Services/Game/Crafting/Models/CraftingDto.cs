using System;
using AutoMapper;

namespace Izumi.Services.Game.Crafting.Models
{
    public record CraftingDto(
        Guid Id,
        long AutoIncrementedId,
        string Name);

    public class CraftingProfile : Profile
    {
        public CraftingProfile() => CreateMap<Data.Entities.Resource.Crafting, CraftingDto>();
    }
}
