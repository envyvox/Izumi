using System;
using AutoMapper;
using Izumi.Data.Enums;

namespace Izumi.Services.Game.Contract.Models
{
    public record ContractDto(
        Guid Id,
        long AutoIncrementedId,
        LocationType Location,
        string Name,
        string Description,
        TimeSpan Duration,
        uint CurrencyReward,
        uint ReputationReward,
        uint EnergyCost);

    public class ContractProfile : Profile
    {
        public ContractProfile() => CreateMap<Data.Entities.Contract, ContractDto>();
    }
}
