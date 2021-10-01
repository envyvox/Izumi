using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Game.Fish.Queries
{
    public record GetFishByIncIdQuery(long IncId) : IRequest<FishDto>;

    public class GetFishByIncIdHandler : IRequestHandler<GetFishByIncIdQuery, FishDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetFishByIncIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<FishDto> Handle(GetFishByIncIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Fishes
                .SingleOrDefaultAsync(x => x.AutoIncrementedId == request.IncId);

            if (entity is null)
            {
                throw new GameUserExpectedException("никогда не слышала о рыбе с таким номером.");
            }

            return _mapper.Map<FishDto>(entity);
        }
    }
}
