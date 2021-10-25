using Izumi.Data.Enums.Discord;

namespace Izumi.Services.Discord.Guild.Models
{
    public record ChannelDto(
        ulong Id,
        DiscordChannelType Type);
}
