using System;
using System.Collections.Generic;
using AutoMapper;
using Izumi.Data.Entities.Resource.Properties;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Gathering.Models
{
    public record GatheringDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        LocationType Location,
        uint Price,
        List<GatheringProperty> Properties);

    public class GatheringProfile : Profile
    {
        public GatheringProfile() => CreateMap<Data.Entities.Resource.Gathering, GatheringDto>();
    }
}
