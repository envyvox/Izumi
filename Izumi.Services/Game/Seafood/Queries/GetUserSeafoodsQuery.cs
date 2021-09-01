using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Seafood.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seafood.Queries
{
    public record GetUserSeafoodsQuery(long UserId) : IRequest<List<UserSeafoodDto>>;

    public class GetUserSeafoodsHandler : IRequestHandler<GetUserSeafoodsQuery, List<UserSeafoodDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserSeafoodsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserSeafoodDto>> Handle(GetUserSeafoodsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserSeafoods
                .Include(x => x.Seafood)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserSeafoodDto>>(entities);
        }
    }
}
