using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Field.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Field.Queries
{
    public record GetUserFieldsQuery(long UserId) : IRequest<List<UserFieldDto>>;

    public class GetUserFieldsHandler : IRequestHandler<GetUserFieldsQuery, List<UserFieldDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserFieldsHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserFieldDto>> Handle(GetUserFieldsQuery request, CancellationToken ct)
        {
            var entities = await _db.UserFields
                .Include(x => x.Seed)
                .ThenInclude(x => x.Crop)
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Number)
                .ToListAsync();

            return _mapper.Map<List<UserFieldDto>>(entities);
        }
    }
}
