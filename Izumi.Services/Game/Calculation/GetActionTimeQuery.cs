using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Izumi.Services.Game.Calculation
{
    public record GetActionTimeQuery(TimeSpan Duration, uint Energy) : IRequest<TimeSpan>;

    public class GetActionTimeHandler : IRequestHandler<GetActionTimeQuery, TimeSpan>
    {
        public async Task<TimeSpan> Handle(GetActionTimeQuery request, CancellationToken ct)
        {
            var dict = new Dictionary<uint, long>
            {
                { 0, request.Duration.Ticks + request.Duration.Ticks * 50 / 100 },
                { 10, request.Duration.Ticks + request.Duration.Ticks * 25 / 100 },
                { 40, request.Duration.Ticks },
                { 70, request.Duration.Ticks - request.Duration.Ticks * 25 / 100 },
                { 85, request.Duration.Ticks - request.Duration.Ticks * 50 / 100 }
            };

            return await Task.FromResult(TimeSpan.FromTicks(
                dict[dict.Keys.Where(x => x <= request.Energy).Max()]));
        }
    }
}
