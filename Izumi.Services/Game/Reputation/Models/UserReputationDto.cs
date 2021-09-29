using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Reputation.Models
{
    public record UserReputationDto(
        ReputationType Type,
        uint Amount);

    public class UserReputationProfile : Profile
    {
        public UserReputationProfile() => CreateMap<UserReputation, UserReputationDto>();
    }
}
