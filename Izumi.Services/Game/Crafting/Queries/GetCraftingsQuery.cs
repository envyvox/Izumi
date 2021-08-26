using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Crafting.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Queries
{
    public record GetCraftingsQuery : IRequest<List<CraftingDto>>;

    public class GetCraftingsHandler : IRequestHandler<GetCraftingsQuery, List<CraftingDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetCraftingsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<CraftingDto>> Handle(GetCraftingsQuery request, CancellationToken ct)
        {
            var entities = await _db.Craftings
                .ToListAsync();

            return _mapper.Map<List<CraftingDto>>(entities);
        }
    }
}
