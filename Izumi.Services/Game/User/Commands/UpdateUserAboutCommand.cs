using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.User.Commands
{
    public record UpdateUserAboutCommand(long UserId, string NewAbout) : IRequest;

    public class UpdateUserAboutHandler : IRequestHandler<UpdateUserAboutCommand>
    {
        private readonly AppDbContext _db;

        public UpdateUserAboutHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateUserAboutCommand request, CancellationToken ct)
        {
            var entity = await _db.Users
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            entity.About = request.NewAbout;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
