using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Seafood.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Seafood.Commands
{
    public record CreateSeafoodCommand(string Name, uint Price) : IRequest<SeafoodDto>;

    public class CreateSeafoodHandler : IRequestHandler<CreateSeafoodCommand, SeafoodDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CreateSeafoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<SeafoodDto> Handle(CreateSeafoodCommand request, CancellationToken ct)
        {
            var exist = await _db.Seafoods
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"seafood with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Seafood
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price
            });

            return _mapper.Map<SeafoodDto>(created);
        }
    }
}
