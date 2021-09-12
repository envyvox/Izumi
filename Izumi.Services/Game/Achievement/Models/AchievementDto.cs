using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Achievement.Models
{
    public record AchievementDto(
        AchievementType Type,
        AchievementCategoryType Category,
        string Name,
        AchievementRewardType RewardType,
        uint RewardNumber);

    public class AchievementProfile : Profile
    {
        public AchievementProfile() => CreateMap<Data.Entities.Achievement, AchievementDto>();
    }
}
