using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crop.Commands
{
    public record CreateCropCommand(string Name, uint Price, Guid SeedId) : IRequest<CropDto>;

    public class CreateCropHandler : IRequestHandler<CreateCropCommand, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateCropHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CropDto> Handle(CreateCropCommand request, CancellationToken ct)
        {
            var exist = await _db.Crops
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"crop with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Crop
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                SeedId = request.SeedId
            });

            return _mapper.Map<CropDto>(created);
        }
    }
}
