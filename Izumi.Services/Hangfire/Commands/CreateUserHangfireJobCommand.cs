using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Hangfire.Commands
{
    public record CreateUserHangfireJobCommand(
            long UserId,
            HangfireJobType Type,
            string JobId,
            DateTimeOffset Expiration)
        : IRequest;

    public class CreateUserHangfireJobHandler : IRequestHandler<CreateUserHangfireJobCommand>
    {
        private readonly AppDbContext _db;

        public CreateUserHangfireJobHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateUserHangfireJobCommand request, CancellationToken cancellationToken)
        {
            var exist = await _db.UserHangfireJobs
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already have hangfire job with type {request.Type.ToString()}");
            }

            await _db.CreateEntity(new UserHangfireJob
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                JobId = request.JobId,
                CreatedAt = DateTimeOffset.UtcNow,
                Expiration = request.Expiration
            });

            return Unit.Value;
        }
    }
}
