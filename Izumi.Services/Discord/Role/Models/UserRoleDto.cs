using System;
using AutoMapper;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Discord.Role.Models
{
    public record UserRoleDto(
        Guid Id,
        DateTimeOffset Expiration,
        User User,
        Data.Entities.Discord.Role Role);

    public class UserRoleProfile : Profile
    {
        public UserRoleProfile() => CreateMap<UserRole, UserRoleDto>();
    }
}
