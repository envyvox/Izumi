using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Crafting.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Commands
{
    public record UpdateCraftingPropertyCommand(
            Guid CraftingId,
            CraftingPropertyType Property,
            uint Value)
        : IRequest<CraftingPropertyDto>;

    public class UpdateCraftingPropertyHandler : IRequestHandler<UpdateCraftingPropertyCommand, CraftingPropertyDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateCraftingPropertyHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CraftingPropertyDto> Handle(UpdateCraftingPropertyCommand request, CancellationToken ct)
        {
            var entity = await _db.CraftingProperties
                .Include(x => x.Crafting)
                .SingleOrDefaultAsync(x =>
                    x.CraftingId == request.CraftingId &&
                    x.Property == request.Property);

            entity.Value = request.Value;

            await _db.UpdateEntity(entity);

            return _mapper.Map<CraftingPropertyDto>(entity);
        }
    }
}
