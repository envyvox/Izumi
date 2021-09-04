using System;
using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Cooldown.Models
{
    public record UserCooldownDto(
        CooldownType Type,
        DateTimeOffset Expiration);

    public class UserCooldownProfile : Profile
    {
        public UserCooldownProfile() => CreateMap<UserCooldown, UserCooldownDto>();
    }
}
