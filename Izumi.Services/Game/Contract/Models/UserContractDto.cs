using System;
using AutoMapper;
using Izumi.Data.Entities.User;

namespace Izumi.Services.Game.Contract.Models
{
    public record UserContractDto(
        DateTimeOffset Expiration,
        ContractDto Contract);

    public class UserContractProfile : Profile
    {
        public UserContractProfile() => CreateMap<UserContract, UserContractDto>();
    }
}
