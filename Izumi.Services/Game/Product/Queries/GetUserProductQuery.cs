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
    public record GetUserProductQuery(long UserId, Guid ProductId) : IRequest<UserProductDto>;

    public class GetUserProductHandler : IRequestHandler<GetUserProductQuery, UserProductDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserProductHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserProductDto> Handle(GetUserProductQuery request, CancellationToken ct)
        {
            var entity = await _db.UserProducts
                .Include(x => x.Product)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.ProductId == request.ProductId);

            return entity is null
                ? new UserProductDto(null, 0)
                : _mapper.Map<UserProductDto>(entity);
        }
    }
}
