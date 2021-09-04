using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Gathering.Models
{
    public record UserGatheringDto(
        Data.Entities.Resource.Gathering Gathering,
        uint Amount);

    public class UserGatheringProfile : Profile
    {
        public UserGatheringProfile() => CreateMap<UserGathering, UserGatheringDto>();
    }
}
