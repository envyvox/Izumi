using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Discord.CommunityDesc.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.CommunityDesc.Queries
{
    public record GetContentMessagesByUserIdQuery(long UserId) : IRequest<List<ContentMessageDto>>;

    public class GetContentMessagesByUserIdHandler
        : IRequestHandler<GetContentMessagesByUserIdQuery, List<ContentMessageDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetContentMessagesByUserIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<ContentMessageDto>> Handle(GetContentMessagesByUserIdQuery request, CancellationToken ct)
        {
            var entities = await _db.ContentMessages
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<ContentMessageDto>>(entities);
        }
    }
}
