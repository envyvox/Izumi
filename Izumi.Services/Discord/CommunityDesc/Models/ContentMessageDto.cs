using System;
using AutoMapper;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Discord.CommunityDesc.Models
{
    public record ContentMessageDto(
        Guid Id,
        long ChannelId,
        long MessageId,
        User User);

    public class ContentMessageProfile : Profile
    {
        public ContentMessageProfile() => CreateMap<ContentMessage, ContentMessageDto>();
    }
}
