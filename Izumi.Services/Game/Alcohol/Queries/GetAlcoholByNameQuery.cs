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
    public record GetAlcoholByNameQuery(string Name) : IRequest<AlcoholDto>;

    public class GetAlcoholByNameHandler : IRequestHandler<GetAlcoholByNameQuery, AlcoholDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetAlcoholByNameHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<AlcoholDto> Handle(GetAlcoholByNameQuery request, CancellationToken ct)
        {
            var entity = await _db.Alcohols
                .Include(x => x.Ingredients)
                .Include(x => x.Properties)
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                throw new Exception($"alcohol with name {request.Name} not found");
            }

            return _mapper.Map<AlcoholDto>(entity);
        }
    }
}