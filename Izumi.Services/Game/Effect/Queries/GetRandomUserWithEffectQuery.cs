using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Effect.Queries
{
    public record GetRandomUserWithEffectQuery(EffectType Type) : IRequest<UserDto>;

    public class GetRandomUserWithEffectHandler : IRequestHandler<GetRandomUserWithEffectQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetRandomUserWithEffectHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetRandomUserWithEffectQuery request, CancellationToken ct)
        {
            var entity = await _db.UserEffects
                .OrderByRandom()
                .Take(1)
                .Include(x => x.User)
                .Select(x => x.User)
                .SingleOrDefaultAsync();

            if (entity is null)
            {
                throw new Exception($"there is no users with effect {request.Type.ToString()}");
            }

            return _mapper.Map<UserDto>(entity);
        }
    }
}
