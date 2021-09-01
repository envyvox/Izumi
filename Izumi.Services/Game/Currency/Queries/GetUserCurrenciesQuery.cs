using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Currency.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Currency.Queries
{
    public record GetUserCurrenciesQuery(long UserId) : IRequest<List<UserCurrencyDto>>;

    public class GetUserCurrenciesHandler : IRequestHandler<GetUserCurrenciesQuery, List<UserCurrencyDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserCurrenciesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<UserCurrencyDto>> Handle(GetUserCurrenciesQuery request, CancellationToken ct)
        {
            var entities = await _db.UserCurrencies
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserCurrencyDto>>(entities);
        }
    }
}
