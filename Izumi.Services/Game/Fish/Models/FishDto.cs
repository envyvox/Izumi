using System;
using System.Collections.Generic;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Fish.Models
{
    public record FishDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        FishRarityType Rarity,
        WeatherType CatchWeather,
        TimesDayType CatchTimesDay,
        List<SeasonType> CatchSeasons,
        uint Price);

    public class FishProfile : Profile
    {
        public FishProfile() => CreateMap<Data.Entities.Resource.Fish, FishDto>();
    }
}
