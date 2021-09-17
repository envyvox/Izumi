using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record UpdateAlcoholPropertyCommand(
            Guid AlcoholId,
            AlcoholPropertyType Property,
            uint Value)
        : IRequest<AlcoholPropertyDto>;

    public class UpdateAlcoholPropertyHandler : IRequestHandler<UpdateAlcoholPropertyCommand, AlcoholPropertyDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateAlcoholPropertyHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<AlcoholPropertyDto> Handle(UpdateAlcoholPropertyCommand request, CancellationToken ct)
        {
            var entity = await _db.AlcoholProperties
                .Include(x => x.Alcohol)
                .SingleOrDefaultAsync(x =>
                    x.AlcoholId == request.AlcoholId &&
                    x.Property == request.Property);

            entity.Value = request.Value;

            await _db.UpdateEntity(entity);

            return _mapper.Map<AlcoholPropertyDto>(entity);
        }
    }
}
