using System;
using AutoMapper;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Transit.Models
{
    public record UserMovementDto(
        LocationType Departure,
        LocationType Destination,
        DateTimeOffset Arrival);

    public class UserMovementProfile : Profile
    {
        public UserMovementProfile() => CreateMap<UserMovement, UserMovementDto>();
    }
}
