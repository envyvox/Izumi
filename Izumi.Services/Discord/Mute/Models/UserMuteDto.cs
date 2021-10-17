using AutoMapper;
using Izumi.Data.Entities.Discord;
using Izumi.Services.Game.User.Models;

namespace Izumi.Services.Discord.Mute.Models
{
    public record UserMuteDto(
        uint Minutes,
        string Reason,
        UserDto Moderator);

    public class UserMuteProfile : Profile
    {
        public UserMuteProfile() => CreateMap<UserMute, UserMuteDto>();
    }
}
