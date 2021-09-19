using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Transit.Models
{
    public record TransitDto(
        Guid Id,
        LocationType Departure,
        LocationType Destination,
        TimeSpan Duration,
        uint Price);

    public class TransitProfile : Profile
    {
        public TransitProfile() => CreateMap<Data.Entities.Transit, TransitDto>();
    }
}
