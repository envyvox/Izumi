using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Food.Commands
{
    public record CreateUserRecipeCommand(long UserId, Guid FoodId) : IRequest;

    public class CreateUserRecipeHandler : IRequestHandler<CreateUserRecipeCommand>
    {
        private readonly ILogger<CreateUserRecipeHandler> _logger;
        private readonly AppDbContext _db;

        public CreateUserRecipeHandler(
            DbContextOptions options,
            ILogger<CreateUserRecipeHandler> logger)
        {
            _db = new AppDbContext(options);
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUserRecipeCommand request, CancellationToken cancellationToken)
        {
            var exist = await _db.UserRecipes
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.FoodId == request.FoodId);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have recipe for food {request.FoodId}");
            }

            await _db.CreateEntity(new UserRecipe
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FoodId = request.FoodId,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user recipe entity for user {UserId} with food {FoodId}",
                request.UserId, request.FoodId);

            return Unit.Value;
        }
    }
}