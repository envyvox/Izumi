using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Box.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Box.Queries
{
    public record GetUserBoxQuery(long UserId, BoxType Box) : IRequest<UserBoxDto>;

    public class GetUserBoxHandler : IRequestHandler<GetUserBoxQuery, UserBoxDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserBoxHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserBoxDto> Handle(GetUserBoxQuery request, CancellationToken ct)
        {
            var entity = await _db.UserBoxes
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Box == request.Box);

            return entity is null
                ? new UserBoxDto(request.Box, 0)
                : _mapper.Map<UserBoxDto>(entity);
        }
    }
}
