using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Alcohol.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Alcohol.Commands
{
    public record CreateAlcoholCommand(string Name) : IRequest<AlcoholDto>;

    public class CreateAlcoholHandler : IRequestHandler<CreateAlcoholCommand, AlcoholDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateAlcoholHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<AlcoholDto> Handle(CreateAlcoholCommand request, CancellationToken ct)
        {
            var exist = await _db.Alcohols
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"alcohol with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Resource.Alcohol
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            });

            return _mapper.Map<AlcoholDto>(created);
        }
    }
}
