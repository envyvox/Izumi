using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Product.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Game.Product.Queries
{
    public record GetProductByIncIdQuery(long IncId) : IRequest<ProductDto>;

    public class GetProductByIncIdHandler : IRequestHandler<GetProductByIncIdQuery, ProductDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetProductByIncIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIncIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Products
                .SingleOrDefaultAsync(x => x.AutoIncrementedId == request.IncId);

            if (entity is null)
            {
                throw new GameUserExpectedException("никогда не слышала о продукте с таким номером");
            }

            return _mapper.Map<ProductDto>(entity);
        }
    }
}
