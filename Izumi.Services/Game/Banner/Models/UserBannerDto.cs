using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Banner.Models
{
    public record UserBannerDto(
        Data.Entities.Banner Banner,
        bool IsActive);

    public class UserBannerProfile : Profile
    {
        public UserBannerProfile() => CreateMap<UserBanner, UserBannerDto>();
    }
}
