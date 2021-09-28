using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Building.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Building.Commands
{
    public record UpdateBuildingCommand(BuildingType Type, string Name, string Description) : IRequest<BuildingDto>;

    public class UpdateBuildingHandler : IRequestHandler<UpdateBuildingCommand, BuildingDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateBuildingHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<BuildingDto> Handle(UpdateBuildingCommand request, CancellationToken ct)
        {
            var entity = await _db.Buildings
                .SingleOrDefaultAsync(x => x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception($"building with type {request.Type.ToString()} not found");
            }

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _db.UpdateEntity(entity);

            return _mapper.Map<BuildingDto>(entity);
        }
    }
}
