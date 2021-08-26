using System;
using AutoMapper;

namespace Izumi.Services.Game.Alcohol.Models
{
    public record AlcoholDto(
        Guid Id,
        long AutoIncrementedId,
        string Name);

    public class AlcoholProfile : Profile
    {
        public AlcoholProfile() => CreateMap<Data.Entities.Resource.Alcohol, AlcoholDto>();
    }
}
