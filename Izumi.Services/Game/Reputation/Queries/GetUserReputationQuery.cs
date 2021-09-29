using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Reputation.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Reputation.Queries
{
    public record GetUserReputationQuery(long UserId, ReputationType Type) : IRequest<UserReputationDto>;

    public class GetUserReputationHandler : IRequestHandler<GetUserReputationQuery, UserReputationDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserReputationHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserReputationDto> Handle(GetUserReputationQuery request, CancellationToken ct)
        {
            var entity = await _db.UserReputations
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            return entity is null
                ? new UserReputationDto(request.Type, 0)
                : _mapper.Map<UserReputationDto>(entity);
        }
    }
}
