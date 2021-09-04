using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.Title.Commands
{
    public record AddTitleToUserCommand(long UserId, TitleType Type) : IRequest;

    public class AddTitleToUserHandler : IRequestHandler<AddTitleToUserCommand>
    {
        private readonly AppDbContext _db;

        public AddTitleToUserHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(AddTitleToUserCommand request, CancellationToken ct)
        {
            var exist = await _db.UserTitles
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have title {request.Type.ToString()}");
            }

            await _db.CreateEntity(new UserTitle
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                CreatedAt = DateTimeOffset.UtcNow
            });

            return Unit.Value;
        }
    }
}
