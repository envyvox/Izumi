using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Crafting.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Queries
{
    public record GetCraftingPropertiesQuery(Guid CraftingId) : IRequest<List<CraftingPropertyDto>>;

    public class GetCraftingPropertiesHandler : IRequestHandler<GetCraftingPropertiesQuery, List<CraftingPropertyDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetCraftingPropertiesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<CraftingPropertyDto>> Handle(GetCraftingPropertiesQuery request, CancellationToken ct)
        {
            var entities = await _db.CraftingProperties
                .Include(x => x.Crafting)
                .Where(x => x.CraftingId == request.CraftingId)
                .ToListAsync();

            return _mapper.Map<List<CraftingPropertyDto>>(entities);
        }
    }
}
