using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Fish.Queries
{
    public record GetFishQuery(Guid Id) : IRequest<FishDto>;

    public class GetFishHandler : IRequestHandler<GetFishQuery, FishDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetFishHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<FishDto> Handle(GetFishQuery request, CancellationToken ct)
        {
            var entity = await _db.Fishes
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"fish {request.Id} not found");
            }

            return _mapper.Map<FishDto>(entity);
        }
    }
}
