using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Crafting.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Commands
{
    public record UpdateCraftingCommand(Guid Id, string Name) : IRequest<CraftingDto>;

    public class UpdateCraftingHandler : IRequestHandler<UpdateCraftingCommand, CraftingDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateCraftingHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CraftingDto> Handle(UpdateCraftingCommand request, CancellationToken ct)
        {
            var entity = await _db.Craftings
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;

            await _db.UpdateEntity(entity);

            return _mapper.Map<CraftingDto>(entity);
        }
    }
}
