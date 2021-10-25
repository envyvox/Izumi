using Izumi.Data.Enums.Discord;

namespace Izumi.Services.Discord.Guild.Models
{
    public record RoleDto(
        ulong Id,
        DiscordRoleType Type);
}
