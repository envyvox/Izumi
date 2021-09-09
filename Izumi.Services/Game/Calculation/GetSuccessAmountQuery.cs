using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Izumi.Services.Game.Calculation
{
    public record GetSuccessAmountQuery(uint Chance, uint DoubleChance, uint Amount) : IRequest<uint>;

    public class GetSuccessAmountHandler : IRequestHandler<GetSuccessAmountQuery, uint>
    {
        private readonly Random _random = new();

        public async Task<uint> Handle(GetSuccessAmountQuery request, CancellationToken ct)
        {
            return await Task.FromResult(request.Chance >= _random.Next(1, 101)
                ? request.DoubleChance >= _random.Next(1, 101)
                    ? request.Amount * 2
                    : request.Amount
                : 0);
        }
    }
}
