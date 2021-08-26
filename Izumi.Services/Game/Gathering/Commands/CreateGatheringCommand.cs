using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Gathering.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Gathering.Commands
{
    public record CreateGatheringCommand(string Name, LocationType Location, uint Price) : IRequest<GatheringDto>;

    public class CreateGatheringHandler : IRequestHandler<CreateGatheringCommand, GatheringDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CreateGatheringHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<GatheringDto> Handle(CreateGatheringCommand request, CancellationToken ct)
        {
            var exist = await _db.Gatherings
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"gathering with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Gathering
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Location = request.Location,
                Price = request.Price
            });

            return _mapper.Map<GatheringDto>(created);
        }
    }
}
