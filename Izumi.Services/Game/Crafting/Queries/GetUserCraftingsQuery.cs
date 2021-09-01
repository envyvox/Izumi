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
    public record GetUserCraftingsQuery(long UserId) : IRequest<List<UserCraftingDto>>;

    public class GetUserCraftingsHandler : IRequestHandler<GetUserCraftingsQuery, List<UserCraftingDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserCraftingsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserCraftingDto>> Handle(GetUserCraftingsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserCraftings
                .Include(x => x.Crafting)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserCraftingDto>>(entities);
        }
    }
}
