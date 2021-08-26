using System;
using AutoMapper;

namespace Izumi.Services.Game.Seafood.Models
{
    public record SeafoodDto(
        Guid Id,
        long AutoIncrementedId,
        string Name,
        uint Price);

    public class SeafoodProfile : Profile
    {
        public SeafoodProfile() => CreateMap<Data.Entities.Resource.Seafood, SeafoodDto>();
    }
}
