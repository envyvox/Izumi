using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Seafood.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seafood.Queries
{
    public record GetUserSeafoodQuery(long UserId, Guid SeafoodId) : IRequest<UserSeafoodDto>;

    public class GetUserSeafoodHandler : IRequestHandler<GetUserSeafoodQuery, UserSeafoodDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserSeafoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserSeafoodDto> Handle(GetUserSeafoodQuery request, CancellationToken ct)
        {
            var entity = await _db.UserSeafoods
                .Include(x => x.Seafood)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.SeafoodId == request.SeafoodId);

            return entity is null
                ? new UserSeafoodDto(null, 0)
                : _mapper.Map<UserSeafoodDto>(entity);
        }
    }
}
