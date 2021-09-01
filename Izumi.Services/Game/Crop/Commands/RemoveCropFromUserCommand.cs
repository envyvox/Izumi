using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Crop.Commands
{
    public record RemoveCropFromUserCommand(long UserId, Guid CropId, uint Amount) : IRequest;

    public class RemoveCropFromUserHandler : IRequestHandler<RemoveCropFromUserCommand>
    {
        private readonly AppDbContext _db;

        public RemoveCropFromUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(RemoveCropFromUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserCrops
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.CropId == request.CropId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have entity with crop {request.CropId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
