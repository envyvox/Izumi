using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Statistic.Models
{
    public record UserStatisticDto(
        StatisticType Type,
        uint Amount);

    public class UserStatisticProfile : Profile
    {
        public UserStatisticProfile() => CreateMap<UserStatistic, UserStatisticDto>();
    }
}
