using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Gathering.Models
{
    public record GatheringDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        LocationType Location,
        uint Price);

    public class GatheringProfile : Profile
    {
        public GatheringProfile() => CreateMap<Data.Entities.Resource.Gathering, GatheringDto>();
    }
}
