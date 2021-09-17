using AutoMapper;
using Izumi.Data.Entities;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.World.Models
{
    public record WorldPropertyDto(
        WorldPropertyType Type,
        uint Value);

    public class WorldPropertyProfile : Profile
    {
        public WorldPropertyProfile() => CreateMap<WorldProperty, WorldPropertyDto>();
    }
}
