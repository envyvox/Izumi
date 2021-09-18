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
    public record GetSeafoodsQuery : IRequest<List<SeafoodDto>>;

    public class GetSeafoodsHandler : IRequestHandler<GetSeafoodsQuery, List<SeafoodDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetSeafoodsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<SeafoodDto>> Handle(GetSeafoodsQuery request, CancellationToken ct)
        {
            var entities = await _db.Seafoods
                .AsQueryable()
                .OrderBy(x => x.AutoIncrementedId)
                .ToListAsync();

            return _mapper.Map<List<SeafoodDto>>(entities);
        }
    }
}
