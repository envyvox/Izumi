using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seed.Commands
{
    public record CreateSeedCommand(
            string Name,
            SeasonType Season,
            uint GrowthDays,
            uint ReGrowthDays,
            bool IsMultiply,
            uint Price)
        : IRequest<SeedDto>;

    public class CreateSeedHandler : IRequestHandler<CreateSeedCommand, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateSeedHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<SeedDto> Handle(CreateSeedCommand request, CancellationToken ct)
        {
            var exist = await _db.Seeds
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"seed with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Seed
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Season = request.Season,
                GrowthDays = request.GrowthDays,
                ReGrowthDays = request.ReGrowthDays,
                IsMultiply = request.IsMultiply,
                Price = request.Price
            });

            return _mapper.Map<SeedDto>(created);
        }
    }
}
