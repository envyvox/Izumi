using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Box.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Box.Queries
{
    public record GetUserBoxesQuery(long UserId) : IRequest<Dictionary<BoxType, UserBoxDto>>;

    public class GetUserBoxesHandler : IRequestHandler<GetUserBoxesQuery, Dictionary<BoxType, UserBoxDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserBoxesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<Dictionary<BoxType, UserBoxDto>> Handle(GetUserBoxesQuery request, CancellationToken ct)
        {
            var entity = await _db.UserBoxes
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToDictionaryAsync(x => x.Box);

            return _mapper.Map<Dictionary<BoxType, UserBoxDto>>(entity);
        }
    }
}
