using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record UpdateAlcoholCommand(Guid Id, string Name) : IRequest<AlcoholDto>;

    public class UpdateAlcoholHandler : IRequestHandler<UpdateAlcoholCommand, AlcoholDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateAlcoholHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<AlcoholDto> Handle(UpdateAlcoholCommand request, CancellationToken ct)
        {
            var entity = await _db.Alcohols
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;

            await _db.UpdateEntity(entity);

            return _mapper.Map<AlcoholDto>(entity);
        }
    }
}
