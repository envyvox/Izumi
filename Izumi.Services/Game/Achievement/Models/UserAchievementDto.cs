using System;
using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Achievement.Models
{
    public record UserAchievementDto(
        Data.Entities.Achievement Achievement,
        DateTimeOffset CreatedAt);

    public class UserAchievementProfile : Profile
    {
        public UserAchievementProfile() => CreateMap<UserAchievement, UserAchievementDto>();
    }
}
