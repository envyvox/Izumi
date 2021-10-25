using System.Collections.Generic;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Emote.Models;
using Izumi.Services.Discord.Guild.Models;

namespace Izumi.Services.Extensions
{
    public static class DiscordRepository
    {
        public static Dictionary<DiscordChannelType, ChannelDto> Channels = new();
        public static Dictionary<DiscordRoleType, RoleDto> Roles = new();
        public static Dictionary<string, EmoteDto> Emotes = new();
    }
}
