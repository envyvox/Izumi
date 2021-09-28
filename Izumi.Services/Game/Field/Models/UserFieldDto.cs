using System;
using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Field.Models
{
    public record UserFieldDto(
        Guid Id,
        uint Number,
        FieldStateType State,
        uint Progress,
        bool InReGrowth,
        Data.Entities.Resource.Seed Seed);

    public class UserFieldProfile : Profile
    {
        public UserFieldProfile() => CreateMap<UserField, UserFieldDto>();
    }
}
