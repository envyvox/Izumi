using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Product.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Product.Queries
{
    public record GetProductQuery(Guid Id) : IRequest<ProductDto>;

    public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetProductHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken ct)
        {
            var entity = await _db.Products
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"product with id {request.Id} not found");
            }

            return _mapper.Map<ProductDto>(entity);
        }
    }
}
