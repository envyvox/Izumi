using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Contract.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Contract.Commands
{
    public record CreateContractCommand(
            LocationType Location,
            string Name,
            string Description,
            TimeSpan Duration,
            uint CurrencyReward,
            uint ReputationReward,
            uint EnergyCost)
        : IRequest<ContractDto>;

    public class CreateContractHandler : IRequestHandler<CreateContractCommand, ContractDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateContractHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<ContractDto> Handle(CreateContractCommand request, CancellationToken ct)
        {
            var created = await _db.CreateEntity(new Data.Entities.Contract
            {
                Id = Guid.NewGuid(),
                Location = request.Location,
                Name = request.Name,
                Description = request.Description,
                Duration = request.Duration,
                CurrencyReward = request.CurrencyReward,
                ReputationReward = request.ReputationReward,
                EnergyCost = request.EnergyCost
            });

            return _mapper.Map<ContractDto>(created);
        }
    }
}
