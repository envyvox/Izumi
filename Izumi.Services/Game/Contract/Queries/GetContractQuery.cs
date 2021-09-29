using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Contract.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Izumi.Services.Extensions.ExceptionExtensions;

namespace Izumi.Services.Game.Contract.Queries
{
    public record GetContractQuery(long IncId) : IRequest<ContractDto>;

    public class GetContractHandler : IRequestHandler<GetContractQuery, ContractDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetContractHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<ContractDto> Handle(GetContractQuery request, CancellationToken ct)
        {
            var entity = await _db.Contracts
                .SingleOrDefaultAsync(x => x.AutoIncrementedId == request.IncId);

            if (entity is null)
            {
                throw new GameUserExpectedException("никогда не слышала о рабочем контракте с таким номером.");
            }

            return _mapper.Map<ContractDto>(entity);
        }
    }
}
