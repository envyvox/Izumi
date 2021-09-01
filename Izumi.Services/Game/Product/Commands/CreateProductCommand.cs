using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Product.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Product.Commands
{
    public record CreateProductCommand(string Name, uint Price) : IRequest<ProductDto>;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateProductHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var exist = await _db.Products
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"product with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price
            });

            return _mapper.Map<ProductDto>(created);
        }
    }
}
