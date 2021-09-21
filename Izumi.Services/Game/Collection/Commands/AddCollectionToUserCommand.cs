using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.Achievement.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Collection.Commands
{
    public record AddCollectionToUserCommand(long UserId, CollectionType Type, Guid ItemId) : IRequest;

    public class AddCollectionToUserHandler : IRequestHandler<AddCollectionToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddCollectionToUserHandler> _logger;
        private readonly AppDbContext _db;

        public AddCollectionToUserHandler(
            DbContextOptions options,
            IMediator mediator,
            ILogger<AddCollectionToUserHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddCollectionToUserCommand request, CancellationToken ct)
        {
            var exist = await _db.UserCollections
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type &&
                    x.ItemId == request.ItemId);

            if (exist) return Unit.Value;

            await _db.CreateEntity(new UserCollection
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                ItemId = request.ItemId,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Added collection to user {UserId} with type {Type} and item {ItemId}",
                request.UserId, request.Type.ToString(), request.ItemId);

            return await _mediator.Send(new CheckAchievementInUserCommand(request.UserId, request.Type switch
            {
                CollectionType.Gathering => AchievementType.CompleteCollectionGathering,
                CollectionType.Crafting => AchievementType.CompleteCollectionCrafting,
                CollectionType.Alcohol => AchievementType.CompleteCollectionAlcohol,
                CollectionType.Drink => AchievementType.CompleteCollectionDrink,
                CollectionType.Crop => AchievementType.CompleteCollectionCrop,
                CollectionType.Fish => AchievementType.CompleteCollectionFish,
                CollectionType.Food => AchievementType.CompleteCollectionFood,
                _ => throw new ArgumentOutOfRangeException()
            }));
        }
    }
}
