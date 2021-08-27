using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Food.Models
{
    public record UserFoodDto(
        Data.Entities.Resource.Food Food,
        uint Amount);

    public class UserFoodProfile : Profile
    {
        public UserFoodProfile() => CreateMap<UserFood, UserFoodDto>();
    }
}
