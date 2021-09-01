using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.Image.Queries
{
    public record GetImageUrlQuery(ImageType Type) : IRequest<string>;

    public class GetImageUrlHandler : IRequestHandler<GetImageUrlQuery, string>
    {
        private readonly AppDbContext _db;

        public GetImageUrlHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<string> Handle(GetImageUrlQuery request, CancellationToken ct)
        {
            var entity = await _db.Images
                .OrderByRandom()
                .Where(x => x.Type == request.Type)
                .Take(1)
                .Select(x => x.Url)
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new Exception($"no images found for {request.Type.ToString()}");
            }

            return entity;
        }
    }
}
