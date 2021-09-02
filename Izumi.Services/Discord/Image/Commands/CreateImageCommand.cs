using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Discord.Image.Commands
{
    public record CreateImageCommand(ImageType Type, string Url) : IRequest;

    public class CreateImageHandler : IRequestHandler<CreateImageCommand>
    {
        private readonly AppDbContext _db;

        public CreateImageHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateImageCommand request, CancellationToken ct)
        {
            var exist = await _db.Images
                .AnyAsync(x =>
                    x.Type == request.Type &&
                    x.Url == request.Url);

            if (exist)
            {
                throw new Exception($"image {request.Type.ToString()} with url {request.Url} already exist");
            }

            await _db.CreateEntity(new Data.Entities.Discord.Image
            {
                Id = Guid.NewGuid(),
                Type = request.Type,
                Url = request.Url
            });

            return Unit.Value;
        }
    }
}
