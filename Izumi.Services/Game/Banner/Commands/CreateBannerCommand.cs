using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Commands
{
    public record CreateBannerCommand(
            string Name,
            BannerRarityType Rarity,
            uint Price,
            string Url)
        : IRequest<BannerDto>;

    public class CreateBannerHandler : IRequestHandler<CreateBannerCommand, BannerDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public CreateBannerHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<BannerDto> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {
            var exist = await _db.Banners
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"banner with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Banner
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Rarity = request.Rarity,
                Price = request.Price,
                Url = request.Url
            });

            return _mapper.Map<BannerDto>(created);
        }
    }
}
