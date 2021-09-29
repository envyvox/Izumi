using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Contract.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Contract.Queries
{
    public record GetContractsInLocationQuery(LocationType Location) : IRequest<List<ContractDto>>;

    public class GetContractsInLocationHandler : IRequestHandler<GetContractsInLocationQuery, List<ContractDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetContractsInLocationHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<ContractDto>> Handle(GetContractsInLocationQuery request, CancellationToken ct)
        {
            var entities = await _db.Contracts
                .AsQueryable()
                .Where(x => x.Location == request.Location)
                .ToListAsync();

            return _mapper.Map<List<ContractDto>>(entities);
        }
    }
}
