using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Building.Commands
{
    public record AddBuildingToUserCommand(long UserId, BuildingType BuildingType) : IRequest;

    public class AddBuildingToUserHandler : IRequestHandler<AddBuildingToUserCommand>
    {
        private readonly ILogger<AddBuildingToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddBuildingToUserHandler(
            DbContextOptions options,
            ILogger<AddBuildingToUserHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(AddBuildingToUserCommand request, CancellationToken ct)
        {
            var exist = await _db.UserBuildings
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.BuildingType == request.BuildingType);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have {request.BuildingType.ToString()}");
            }

            await _db.CreateEntity(new UserBuilding
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                BuildingType = request.BuildingType,
                Durability = 100,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user building entity for user {UserId} with building {Type}",
                request.UserId, request.BuildingType);

            return Unit.Value;
        }
    }
}
