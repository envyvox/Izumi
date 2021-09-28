using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Field.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Game.Field.Queries
{
    public record GetUserFieldQuery(long UserId, uint Number) : IRequest<UserFieldDto>;

    public class GetUserFieldHandler : IRequestHandler<GetUserFieldQuery, UserFieldDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserFieldHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserFieldDto> Handle(GetUserFieldQuery request, CancellationToken cancellationToken)
        {
            var entity = await _db.UserFields
                .Include(x => x.Seed)
                .ThenInclude(x => x.Crop)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.Number == request.Number)
                .SingleOrDefaultAsync();

            if (entity is null)
            {
                throw new GameUserExpectedException("у тебя нет клетки участка земли с таким номером.");
            }

            return _mapper.Map<UserFieldDto>(entity);
        }
    }
}
