using System;
using AutoMapper;
using Izumi.Data.Entities.Discord;

namespace Izumi.Services.Discord.Role.Models
{
    public record UserRoleDto(
        Guid Id,
        long RoleId,
        DateTimeOffset Expiration);

    public class UserRoleProfile : Profile
    {
        public UserRoleProfile() => CreateMap<UserRole, UserRoleDto>();
    }
}
