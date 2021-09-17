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
    public record UpdateGatheringPropertyCommand(
            Guid GatheringId,
            GatheringPropertyType Property,
            uint Value)
        : IRequest<GatheringPropertyDto>;

    public class UpdateGatheringPropertyHandler : IRequestHandler<UpdateGatheringPropertyCommand, GatheringPropertyDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateGatheringPropertyHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<GatheringPropertyDto> Handle(UpdateGatheringPropertyCommand request, CancellationToken ct)
        {
            var entity = await _db.GatheringProperties
                .Include(x => x.Gathering)
                .SingleOrDefaultAsync(x =>
                    x.GatheringId == request.GatheringId &&
                    x.Property == request.Property);

            entity.Value = request.Value;

            await _db.UpdateEntity(entity);

            return _mapper.Map<GatheringPropertyDto>(entity);
        }
    }
}
