using System;
using System.Collections.Generic;
using System.Linq;
using Izumi.Data.Enums.Discord;
using Izumi.Services.Discord.Guild.Models;

namespace Izumi.Services.Discord.Guild.Extensions
{
    public static class ChannelsExtensions
    {
        public static IEnumerable<ulong> GetCommunityDescChannels(
            this Dictionary<DiscordChannelType, ChannelDto> channels)
        {
            var communityDescChannels = Enum
                .GetValues(typeof(DiscordChannelType))
                .Cast<DiscordChannelType>()
                .Where(x => x.Parent() == DiscordChannelType.CommunityDescParent);

            return channels
                .Where(x => communityDescChannels.Contains(x.Key))
                .Select(x => x.Value.Id);
        }
    }
}
