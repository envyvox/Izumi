using AutoMapper;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Enums.Discord;

namespace Izumi.Services.Discord.Guild.Models
{
    public record ChannelDto(
        long Id,
        DiscordChannelType Type);

    public class ChannelProfile : Profile
    {
        public ChannelProfile() => CreateMap<Channel, ChannelDto>();
    }
}
