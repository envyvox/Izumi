using System;
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
    public record GetUserCraftingQuery(long UserId, Guid CraftingId) : IRequest<UserCraftingDto>;

    public class GetUserCraftingHandler : IRequestHandler<GetUserCraftingQuery, UserCraftingDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserCraftingHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserCraftingDto> Handle(GetUserCraftingQuery request, CancellationToken ct)
        {
            var entity = await _db.UserAlcohols
                .Include(x => x.AlcoholId)
                .Where(x => x.UserId == request.UserId)
                .SingleOrDefaultAsync();

            return entity is null
                ? new UserCraftingDto(null, 0)
                : _mapper.Map<UserCraftingDto>(entity);
        }
    }
}
