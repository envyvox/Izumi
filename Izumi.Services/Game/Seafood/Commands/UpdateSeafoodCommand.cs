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
    public record UpdateSeafoodCommand(
            Guid Id,
            string Name,
            uint Price)
        : IRequest<SeafoodDto>;

    public class UpdateSeafoodHandler : IRequestHandler<UpdateSeafoodCommand, SeafoodDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UpdateSeafoodHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<SeafoodDto> Handle(UpdateSeafoodCommand request, CancellationToken ct)
        {
            var entity = await _db.Seafoods
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            entity.Name = request.Name;
            entity.Price = request.Price;

            await _db.UpdateEntity(entity);

            return _mapper.Map<SeafoodDto>(entity);
        }
    }
}
