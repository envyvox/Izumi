using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Gathering.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Commands
{
    public record UpdateGatheringCommand(
            Guid Id,
            string Name,
            LocationType Location,
            uint Price)
        : IRequest<GatheringDto>;

    public class UpdateGatheringHandler : IRequestHandler<UpdateGatheringCommand, GatheringDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateGatheringHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<GatheringDto> Handle(UpdateGatheringCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.Gatherings
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.Location = request.Location;
            entity.Price = request.Price;

            await _db.UpdateEntity(entity);

            return _mapper.Map<GatheringDto>(entity);
        }
    }
}
