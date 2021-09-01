using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Seed.Models
{
    public record UserSeedDto(
        Data.Entities.Resource.Seed Seed,
        uint Amount);

    public class UserSeedProfile : Profile
    {
        public UserSeedProfile() => CreateMap<UserSeed, UserSeedDto>();
    }
}
