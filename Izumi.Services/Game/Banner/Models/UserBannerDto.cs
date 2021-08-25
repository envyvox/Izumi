using System;
using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Banner.Models
{
    public record UserBannerDto(
        Guid Id,
        long UserId,
        Data.Entities.User.User User,
        Guid BannerId,
        Data.Entities.Banner Banner,
        bool IsActive,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class UserBannerProfile : Profile
    {
        public UserBannerProfile() => CreateMap<UserBanner, UserBannerDto>();
    }
}
