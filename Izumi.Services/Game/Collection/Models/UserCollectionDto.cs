using System;
using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Collection.Models
{
    public record UserCollectionDto(
        CollectionType Type,
        Guid ItemId,
        DateTimeOffset CreatedAt);

    public class UserCollectionProfile : Profile
    {
        public UserCollectionProfile() => CreateMap<UserCollection, UserCollectionDto>();
    }
}
