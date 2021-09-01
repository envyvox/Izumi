using AutoMapper;
using Izumi.Data.Enums.Discord;

namespace Izumi.Services.Discord.Guild.Models
{
    public record RoleDto(
        long Id,
        DiscordRoleType Type);

    public class RoleProfile : Profile
    {
        public RoleProfile() => CreateMap<Data.Entities.Discord.Role, RoleDto>();
    }
}
