using System;
using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Building.Models
{
    public record UserBuildingDto(
        Guid Id,
        uint Durability,
        BuildingDto Building);

    public class UserBuildingProfile : Profile
    {
        public UserBuildingProfile() => CreateMap<UserBuilding, UserBuildingDto>();
    }
}
