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
    public record UpdateSeedCommand(
            Guid Id,
            string Name,
            SeasonType Season,
            uint GrowthDays,
            uint ReGrowthDays,
            bool IsMultiply,
            uint Price)
        : IRequest<SeedDto>;

    public class UpdateSeedHandler : IRequestHandler<UpdateSeedCommand, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateSeedHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<SeedDto> Handle(UpdateSeedCommand request, CancellationToken ct)
        {
            var entity = await _db.Seeds
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.Season = request.Season;
            entity.GrowthDays = request.GrowthDays;
            entity.ReGrowthDays = request.ReGrowthDays;
            entity.IsMultiply = request.IsMultiply;
            entity.Price = request.Price;

            await _db.UpdateEntity(entity);

            return _mapper.Map<SeedDto>(entity);
        }
    }
}
