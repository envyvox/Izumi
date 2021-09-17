using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Entities;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.World.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.World.Commands
{
    public record UpdateWorldPropertyCommand(WorldPropertyType Type, uint Value) : IRequest<WorldPropertyDto>;

    public class UpdateWorldPropertyHandler : IRequestHandler<UpdateWorldPropertyCommand, WorldPropertyDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateWorldPropertyHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<WorldPropertyDto> Handle(UpdateWorldPropertyCommand request, CancellationToken ct)
        {
            var entity = await _db.WorldProperties
                .SingleOrDefaultAsync(x => x.Type == request.Type);

            if (entity is null)
            {
                var created = await _db.CreateEntity(new WorldProperty
                {
                    Type = request.Type,
                    Value = request.Value
                });

                return _mapper.Map<WorldPropertyDto>(created);
            }

            entity.Value = request.Value;

            await _db.UpdateEntity(entity);

            return _mapper.Map<WorldPropertyDto>(entity);
        }
    }
}
