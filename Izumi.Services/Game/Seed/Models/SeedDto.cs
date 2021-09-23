using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Seed.Models
{
    public record SeedDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        SeasonType Season,
        uint GrowthDays,
        uint ReGrowthDays,
        bool IsMultiply,
        uint Price,
        Data.Entities.Resource.Crop Crop);

    public class SeedProfile : Profile
    {
        public SeedProfile() => CreateMap<Data.Entities.Resource.Seed, SeedDto>();
    }
}
