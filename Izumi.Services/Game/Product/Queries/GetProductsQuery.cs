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
    public record GetProductsQuery : IRequest<List<ProductDto>>;

    public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetProductsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
        {
            var entities = await _db.Products
                .AsQueryable()
                .OrderBy(x => x.AutoIncrementedId)
                .ToListAsync();

            return _mapper.Map<List<ProductDto>>(entities);
        }
    }
}
