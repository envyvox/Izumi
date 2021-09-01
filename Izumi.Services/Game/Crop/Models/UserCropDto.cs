using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Crop.Models
{
    public record UserCropDto(
        Data.Entities.Resource.Crop Crop,
        uint Amount);

    public class UserCropProfile : Profile
    {
        public UserCropProfile() => CreateMap<UserCrop, UserCropDto>();
    }
}
