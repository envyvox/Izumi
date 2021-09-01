using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Services.Game.Currency.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Currency.Queries
{
    public record GetUserCurrencyQuery(long UserId, CurrencyType Currency) : IRequest<UserCurrencyDto>;

    public class GetUserCurrencyHandler : IRequestHandler<GetUserCurrencyQuery, UserCurrencyDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserCurrencyHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<UserCurrencyDto> Handle(GetUserCurrencyQuery request, CancellationToken ct)
        {
            var entity = await _db.UserCurrencies
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Currency == request.Currency);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have currency {request.Currency}");
            }

            return _mapper.Map<UserCurrencyDto>(entity);
        }
    }
}
