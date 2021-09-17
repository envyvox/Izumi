using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Izumi.Data;
using Izumi.Data.Enums;
using Izumi.Data.Enums.Discord;
using Izumi.Data.Extensions;
using Izumi.Services.Discord.Guild.Commands;
using Izumi.Services.Game.Banner.Commands;
using Izumi.Services.Game.Banner.Queries;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Title.Commands;
using Izumi.Services.Game.User.Models;
using Izumi.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Izumi.Services.Game.User.Commands
{
    public record CreateUserCommand(long UserId) : IRequest<UserDto>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private const string DefaultCommandColor = "202225";

        public CreateUserHandler(
            DbContextOptions options,
            IMapper mapper,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var exist = await _db.Users
                .AnyAsync(x => x.Id == request.UserId);

            if (exist)
            {
                throw new Exception($"user with id {request.UserId} already exist");
            }

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

            var banner = await _mediator.Send(new GetBannerByIncIdQuery(1));
            var startup = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldPropertyType.EconomyStartup));

            await _mediator.Send(new AddRoleToGuildUserCommand((ulong) entity.Id, DiscordRoleType.LocationCapital));
            await _mediator.Send(new AddBannerToUserCommand(entity.Id, banner.Id, true));
            await _mediator.Send(new AddTitleToUserCommand(entity.Id, TitleType.Newbie));
            await _mediator.Send(new AddCurrencyToUserCommand(entity.Id, CurrencyType.Ien, startup));

            return _mapper.Map<UserDto>(entity);
        }
    }
}
