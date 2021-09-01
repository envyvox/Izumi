using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Product.Models
{
    public record UserProductDto(
        Data.Entities.Resource.Product Product,
        uint Amount);

    public class UserProductProfile : Profile
    {
        public UserProductProfile() => CreateMap<UserProduct, UserProductDto>();
    }
}
