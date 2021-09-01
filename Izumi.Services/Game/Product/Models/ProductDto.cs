using System;
using AutoMapper;

namespace Izumi.Services.Game.Product.Models
{
    public record ProductDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        uint Price);

    public class ProductProfile : Profile
    {
        public ProductProfile() => CreateMap<Data.Entities.Resource.Product, ProductDto>();
    }
}
