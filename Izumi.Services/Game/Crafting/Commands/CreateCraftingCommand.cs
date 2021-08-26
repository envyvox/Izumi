using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Crafting.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crafting.Commands
{
    public record CreateCraftingCommand(string Name) : IRequest<CraftingDto>;

    public class CreateCraftingHandler : IRequestHandler<CreateCraftingCommand, CraftingDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CreateCraftingHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<CraftingDto> Handle(CreateCraftingCommand request, CancellationToken ct)
        {
            var exist = await _db.Craftings
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"crafting with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Crafting
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            });

            return _mapper.Map<CraftingDto>(created);
        }
    }
}
