using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Localization.Models
{
    public record LocalizationDto(
        Guid Id,
        LocalizationCategoryType Category,
        string Name,
        string Single,
        string Double,
        string Multiply);

    public class LocalizationProfile : Profile
    {
        public LocalizationProfile() => CreateMap<Data.Entities.Localization, LocalizationDto>();
    }
}
