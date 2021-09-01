using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Queries
{
    public record GetUserAlcoholsQuery(long UserId) : IRequest<List<UserAlcoholDto>>;

    public class GetUserAlcoholsHandler : IRequestHandler<GetUserAlcoholsQuery, List<UserAlcoholDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserAlcoholsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserAlcoholDto>> Handle(GetUserAlcoholsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserAlcohols
                .Include(x => x.Alcohol)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserAlcoholDto>>(entities);
        }
    }
}
