using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.User.Commands
{
    public record CreateUserCommand(long UserId) : IRequest<UserDto>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private const string DefaultCommandColor = "202225";

        public CreateUserHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var entity = await _db.CreateEntity(new Data.Entities.User.User
            {
                Id = request.UserId,
                About = null,
                Title = TitleType.Newbie,
                Gender = GenderType.None,
                Location = LocationType.Capital,
                Energy = 100,
                Points = 0,
                IsPremium = false,
                CommandColor = DefaultCommandColor,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            return _mapper.Map<UserDto>(entity);
        }
    }
}
