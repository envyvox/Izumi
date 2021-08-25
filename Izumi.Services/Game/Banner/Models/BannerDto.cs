using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Banner.Models
{
    public record BannerDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        BannerRarityType Rarity,
        uint Price,
        string Url);

    public class BannerProfile : Profile
    {
        public BannerProfile() => CreateMap<Data.Entities.Banner, BannerDto>();
    }
}
