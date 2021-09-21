using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Crop.Commands
{
    public record AddCropToUserCommand(long UserId, Guid CropId, uint Amount) : IRequest;

    public class AddCropToUserHandler : IRequestHandler<AddCropToUserCommand>
    {
        private readonly ILogger<AddCropToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddCropToUserHandler(
            DbContextOptions options,
            ILogger<AddCropToUserHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddCropToUserCommand request, CancellationToken ct)
        {
            var entity = await _db.UserCrops
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.CropId == request.CropId);

            if (entity is null)
            {
                await _db.CreateEntity(new UserCrop
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    CropId = request.CropId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user crop entity for user {UserId} with crop {CropId} and amount {Amount}",
                    request.UserId, request.CropId, request.Amount);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} crop {CropId} amount {Amount}",
                    request.UserId, request.CropId, request.Amount);
            }

            return Unit.Value;
        }
    }
}
