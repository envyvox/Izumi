using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Alcohol.Models
{
    public record UserAlcoholDto(
        Data.Entities.Resource.Alcohol Alcohol,
        uint Amount);

    public class UserAlcoholProfile : Profile
    {
        public UserAlcoholProfile() => CreateMap<UserAlcohol, UserAlcoholDto>();
    }
}
