using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Queries
{
    public record GetAlcoholByIdQuery(Guid Id) : IRequest<AlcoholDto>;

    public class GetAlcoholHandler : IRequestHandler<GetAlcoholByIdQuery, AlcoholDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetAlcoholHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<AlcoholDto> Handle(GetAlcoholByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Alcohols
                .Include(x => x.Properties)
                .Include(x => x.Ingredients)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"alcohol {request.Id} not found");
            }

            return _mapper.Map<AlcoholDto>(entity);
        }
    }
}
