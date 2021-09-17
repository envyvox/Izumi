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
    public record UpdateProductCommand(
            Guid Id,
            string Name,
            uint Price)
        : IRequest<ProductDto>;

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateProductHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var entity = await _db.Products
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.Price = request.Price;

            await _db.UpdateEntity(entity);

            return _mapper.Map<ProductDto>(entity);
        }
    }
}
