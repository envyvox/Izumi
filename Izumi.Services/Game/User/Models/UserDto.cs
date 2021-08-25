using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.User.Models
{
    public record UserDto(
        long Id,
        string About,
        TitleType Title,
        GenderType Gender,
        LocationType Location,
        uint Energy,
        uint Points,
        bool IsPremium,
        string CommandColor,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class UserProfile : Profile
    {
        public UserProfile() => CreateMap<Data.Entities.User.User, UserDto>();
    }
}
