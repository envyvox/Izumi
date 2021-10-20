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
    public record GetUserAlcoholQuery(long UserId, Guid AlcoholId) : IRequest<UserAlcoholDto>;

    public class GetUserAlcoholHandler : IRequestHandler<GetUserAlcoholQuery, UserAlcoholDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserAlcoholHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserAlcoholDto> Handle(GetUserAlcoholQuery request, CancellationToken cancellationToken)
        {
            var entity = await _db.UserAlcohols
                .Include(x => x.Alcohol)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.AlcoholId == request.AlcoholId);

            return entity is null
                ? new UserAlcoholDto(null, 0)
                : _mapper.Map<UserAlcoholDto>(entity);
        }
    }
}
