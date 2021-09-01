using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Box.Models
{
    public record UserBoxDto(
        BoxType Box,
        uint Amount);

    public class UserBoxProfile : Profile
    {
        public UserBoxProfile() => CreateMap<UserBox, UserBoxDto>();
    }
}
