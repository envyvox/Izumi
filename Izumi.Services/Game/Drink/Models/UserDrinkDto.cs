using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Drink.Models
{
    public record UserDrinkDto(
        Data.Entities.Resource.Drink Drink,
        uint Amount);

    public class UserDrinkProfile : Profile
    {
        public UserDrinkProfile() => CreateMap<UserDrink, UserDrinkDto>();
    }
}
