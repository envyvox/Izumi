using System;
using AutoMapper;

namespace Izumi.Services.Game.Crop.Models
{
    public record CropDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        uint Price,
        Data.Entities.Resource.Seed Seed);

    public class CropProfile : Profile
    {
        public CropProfile() => CreateMap<Data.Entities.Resource.Crop, CropDto>();
    }
}
