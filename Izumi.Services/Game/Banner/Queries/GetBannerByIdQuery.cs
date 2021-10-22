using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Banner.Queries
{
    public record GetBannerByIdQuery(Guid Id) : IRequest<BannerDto>;

    public class GetBannerByIdHandler : IRequestHandler<GetBannerByIdQuery, BannerDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetBannerByIdHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<BannerDto> Handle(GetBannerByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Banners
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception($"banner with id {request.Id} not found");
            }

            return _mapper.Map<BannerDto>(entity);
        }
    }
}
