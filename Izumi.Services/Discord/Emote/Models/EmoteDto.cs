using System;
using AutoMapper;

namespace Izumi.Services.Discord.Emote.Models
{
    public record EmoteDto(
        long Id,
        string Name,
        string Code,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class EmoteProfile : Profile
    {
        public EmoteProfile() => CreateMap<Data.Entities.Discord.Emote, EmoteDto>();
    }
}
