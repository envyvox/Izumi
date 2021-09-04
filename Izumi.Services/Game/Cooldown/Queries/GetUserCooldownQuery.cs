using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Cooldown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Cooldown.Queries
{
    public record GetUserCooldownQuery(long UserId, CooldownType Type) : IRequest<UserCooldownDto>;

    public class GetUserCooldownHandler : IRequestHandler<GetUserCooldownQuery, UserCooldownDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserCooldownHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserCooldownDto> Handle(GetUserCooldownQuery request, CancellationToken ct)
        {
            var entity = await _db.UserCooldowns
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            return _mapper.Map<UserCooldownDto>(entity is null
                ? new UserCooldownDto(request.Type, DateTimeOffset.UtcNow)
                : entity);
        }
    }
}
