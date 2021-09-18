using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Crafting.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Queries
{
    public record GetCraftingQuery(Guid Id) : IRequest<CraftingDto>;

    public class GetCraftingHandler : IRequestHandler<GetCraftingQuery, CraftingDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetCraftingHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CraftingDto> Handle(GetCraftingQuery request, CancellationToken ct)
        {
            var entity = await _db.Craftings
                .Include(x => x.Properties)
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"crafting with id {request.Id} not found");
            }

            return _mapper.Map<CraftingDto>(entity);
        }
    }
}
