using System;
using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Effect.Models
{
    public record UserEffectDto(
        EffectType Type,
        DateTimeOffset CreatedAt,
        DateTimeOffset? Expiration);

    public class UserEffectProfile : Profile
    {
        public UserEffectProfile() => CreateMap<UserEffect, UserEffectDto>();
    }
}
