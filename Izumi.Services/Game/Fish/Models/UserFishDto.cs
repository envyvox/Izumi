using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Fish.Models
{
    public record UserFishDto(
        Data.Entities.Resource.Fish Fish,
        uint Amount);

    public class UserFishProfile : Profile
    {
        public UserFishProfile() => CreateMap<UserFish, UserFishDto>();
    }
}
