using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Box.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Box.Queries
{
    public record GetUserBoxesQuery(long UserId) : IRequest<List<UserBoxDto>>;

    public class GetUserBoxesHandler : IRequestHandler<GetUserBoxesQuery, List<UserBoxDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserBoxesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserBoxDto>> Handle(GetUserBoxesQuery request, CancellationToken ct)
        {
            var entity = await _db.UserBoxes
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserBoxDto>>(entity);
        }
    }
}
