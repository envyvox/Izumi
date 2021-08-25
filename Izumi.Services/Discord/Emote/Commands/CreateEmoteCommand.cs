using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Discord.Emote.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.Emote.Commands
{
    public record CreateEmoteCommand(long Id, string Name, string Code) : IRequest<EmoteDto>;

    public class CreateEmoteHandler : IRequestHandler<CreateEmoteCommand, EmoteDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateEmoteHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<EmoteDto> Handle(CreateEmoteCommand request, CancellationToken ct)
        {
            var exist = await _db.Emotes
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"emote with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Discord.Emote
            {
                Id = request.Id,
                Name = request.Name,
                Code = request.Code,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            return _mapper.Map<EmoteDto>(created);
        }
    }
}
