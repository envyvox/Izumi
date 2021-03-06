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
    public record GetSeafoodByIdQuery(Guid Id) : IRequest<SeafoodDto>;

    public class GetSeafoodHandler : IRequestHandler<GetSeafoodByIdQuery, SeafoodDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetSeafoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<SeafoodDto> Handle(GetSeafoodByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Seafoods
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"seafood {request.Id} not found");
            }

            return _mapper.Map<SeafoodDto>(entity);
        }
    }
}
