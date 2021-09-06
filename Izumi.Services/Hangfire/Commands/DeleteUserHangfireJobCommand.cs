using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Hangfire.Commands
{
    public record DeleteUserHangfireJobCommand(long UserId, HangfireJobType Type) : IRequest;

    public class DeleteUserHangfireJobHandler : IRequestHandler<DeleteUserHangfireJobCommand>
    {
        private readonly AppDbContext _db;

        public DeleteUserHangfireJobHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(DeleteUserHangfireJobCommand request, CancellationToken ct)
        {
            var entity = await _db.UserHangfireJobs
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have hangfirejob entiy with type {request.Type.ToString()}");
            }

            await _db.DeleteEntity(entity);

            return Unit.Value;
        }
    }
}
