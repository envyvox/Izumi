using System;
using AutoMapper;
using Izumi.Data.Entities.Discord;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Discord.CommunityDesc.Models
{
    public record ContentVoteDto(
        Guid Id,
        VoteType Vote,
        bool IsActive,
        User User,
        ContentMessage Message);

    public class ContentVoteProfile : Profile
    {
        public ContentVoteProfile() => CreateMap<ContentVote, ContentVoteDto>();
    }
}
