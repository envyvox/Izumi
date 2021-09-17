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
    public record UpdateCropCommand(Guid Id, string Name, uint Price) : IRequest<CropDto>;

    public class UpdateCropHandler : IRequestHandler<UpdateCropCommand, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateCropHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CropDto> Handle(UpdateCropCommand request, CancellationToken ct)
        {
            var entity = await _db.Crops
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.Price = request.Price;

            await _db.UpdateEntity(entity);

            return _mapper.Map<CropDto>(entity);
        }
    }
}
