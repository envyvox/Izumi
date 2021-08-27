using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Crafting.Models
{
    public record UserCraftingDto(
        Data.Entities.Resource.Crafting Crafting,
        uint Amount);

    public class UserCraftingProfile : Profile
    {
        public UserCraftingProfile() => CreateMap<UserCrafting, UserCraftingDto>();
    }
}
