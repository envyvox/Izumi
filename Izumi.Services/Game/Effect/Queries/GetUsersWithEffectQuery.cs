using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Effect.Queries
{
    public record GetUsersWithEffectQuery(EffectType Type) : IRequest<List<UserDto>>;

    public class GetUsersWithEffectHandler : IRequestHandler<GetUsersWithEffectQuery, List<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUsersWithEffectHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetUsersWithEffectQuery request, CancellationToken ct)
        {
            var entities = await _db.UserEffects
                .Include(x => x.User)
                .Where(x => x.Type == request.Type)
                .Select(x => x.User)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(entities);
        }
    }
}
