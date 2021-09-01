using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Product.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Product.Queries
{
    public record GetUserProductsQuery(long UserId) : IRequest<List<UserProductDto>>;

    public class GetUserProductsHandler : IRequestHandler<GetUserProductsQuery, List<UserProductDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserProductsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserProductDto>> Handle(GetUserProductsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserProducts
                .Include(x => x.Product)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserProductDto>>(entities);
        }
    }
}
