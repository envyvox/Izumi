using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Commands
{
    public record CreateBannerCommand(
            string Name,
            BannerRarityType Rarity,
            uint Price,
            string Url)
        : IRequest;

    public class CreateBannerHandler : IRequestHandler<CreateBannerCommand>
    {
        private readonly AppDbContext _db;

        public CreateBannerHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {
            var exist = await _db.Banners
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"banner with name {request.Name} already exist");
            }

            await _db.CreateEntity(new Data.Entities.Banner
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Rarity = request.Rarity,
                Price = request.Price,
                Url = request.Url
            });

            return Unit.Value;
        }
    }
}
