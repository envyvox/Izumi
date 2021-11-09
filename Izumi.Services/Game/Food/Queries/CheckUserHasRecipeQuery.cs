using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Food.Queries
{
    public record CheckUserHasRecipeQuery(long UserId, Guid FoodId) : IRequest<bool>;

    public class CheckUserHasRecipeHandler : IRequestHandler<CheckUserHasRecipeQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserHasRecipeHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserHasRecipeQuery request, CancellationToken ct)
        {
            return await _db.UserRecipes
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.FoodId == request.FoodId);
        }
    }
}