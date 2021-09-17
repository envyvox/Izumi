using AutoMapper;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Gathering.Models
{
    public record GatheringPropertyDto(
        Data.Entities.Resource.Gathering Gathering,
        GatheringPropertyType Property,
        uint Value);

    public class GatheringPropertyProfile : Profile
    {
        public GatheringPropertyProfile() => CreateMap<GatheringProperty, GatheringPropertyDto>();
    }
}
