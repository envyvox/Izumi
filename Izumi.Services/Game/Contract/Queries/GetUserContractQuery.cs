using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Contract.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Contract.Queries
{
    public record GetUserContractQuery(long UserId) : IRequest<UserContractDto>;

    public class GetUserContractHandler : IRequestHandler<GetUserContractQuery, UserContractDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserContractHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserContractDto> Handle(GetUserContractQuery request, CancellationToken ct)
        {
            var entity = await _db.UserContracts
                .Include(x => x.Contract)
                .SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt work on contract");
            }

            return _mapper.Map<UserContractDto>(entity);
        }
    }
}
