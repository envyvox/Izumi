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
    public record GetCraftingByNameQuery(string Name) : IRequest<CraftingDto>;

    public class GetCraftingByNameHandler : IRequestHandler<GetCraftingByNameQuery, CraftingDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetCraftingByNameHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CraftingDto> Handle(GetCraftingByNameQuery request, CancellationToken ct)
        {
            var entity = await _db.Craftings
                .Include(x => x.Ingredients)
                .Include(x => x.Properties)
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                throw new Exception($"crafting with name {request.Name} not found");
            }

            return _mapper.Map<CraftingDto>(entity);
        }
    }
}