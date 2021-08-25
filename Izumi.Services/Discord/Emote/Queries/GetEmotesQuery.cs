using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Discord.Emote.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.Emote.Queries
{
    public record GetEmotesQuery : IRequest<Dictionary<string, EmoteDto>>;

    public class GetEmotesHandler : IRequestHandler<GetEmotesQuery, Dictionary<string, EmoteDto>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetEmotesHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<Dictionary<string, EmoteDto>> Handle(GetEmotesQuery request, CancellationToken ct)
        {
            var entities = await _db.Emotes
                .ToDictionaryAsync(x => x.Name);

            return _mapper.Map<Dictionary<string, EmoteDto>>(entities);
        }
    }
}
