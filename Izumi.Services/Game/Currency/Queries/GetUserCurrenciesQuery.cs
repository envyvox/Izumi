using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Currency.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Currency.Queries
{
    public record GetUserCurrenciesQuery(long UserId) : IRequest<Dictionary<CurrencyType, UserCurrencyDto>>;

    public class GetUserCurrenciesHandler
        : IRequestHandler<GetUserCurrenciesQuery, Dictionary<CurrencyType, UserCurrencyDto>>
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

        public async Task<Dictionary<CurrencyType, UserCurrencyDto>> Handle(GetUserCurrenciesQuery request,
            CancellationToken ct)
        {
            var entities = await _db.UserCurrencies
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToDictionaryAsync(x => x.Currency);

            return _mapper.Map<Dictionary<CurrencyType, UserCurrencyDto>>(entities);
        }
    }
}
