using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Transit.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Transit.Commands
{
    public record UpdateTransitCommand(
            Guid Id,
            TimeSpan Duration,
            uint Price)
        : IRequest<TransitDto>;

    public class UpdateTransitHandler : IRequestHandler<UpdateTransitCommand, TransitDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateTransitHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<TransitDto> Handle(UpdateTransitCommand request, CancellationToken ct)
        {
            var entity = await _db.Transits
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"transit with id {request.Id} not found");
            }

            entity.Duration = request.Duration;
            entity.Price = request.Price;

            await _db.UpdateEntity(entity);

            return _mapper.Map<TransitDto>(entity);
        }
    }
}
