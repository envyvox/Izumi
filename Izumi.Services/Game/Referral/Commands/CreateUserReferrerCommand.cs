using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data;
using Izumi.Data.Entities.User;
using Izumi.Data.Enums;
using Izumi.Data.Extensions;
using Izumi.Services.Discord.Embed;
using Izumi.Services.Discord.Emote.Extensions;
using Izumi.Services.Discord.Emote.Queries;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.Box.Commands;
using Izumi.Services.Game.Currency.Commands;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Referral.Queries;
using Izumi.Services.Game.Title.Commands;
using Izumi.Services.Game.User.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Izumi.Services.Game.Referral.Commands
{
    public record CreateUserReferrerCommand(long UserId, long ReferrerId) : IRequest;

    public class CreateUserReferrerHandler : IRequestHandler<CreateUserReferrerCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly ILogger<CreateUserReferrerHandler> _logger;
        private readonly AppDbContext _db;

        public CreateUserReferrerHandler(
            DbContextOptions options,
            IMediator mediator,
            ILocalizationService local,
            ILogger<CreateUserReferrerHandler> logger)
        {
            _mediator = mediator;
            _local = local;
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateUserReferrerCommand request, CancellationToken ct)
        {
            var exist = await _db.UserReferrers
                .AnyAsync(x => x.UserId == request.UserId);

            if (exist)
            {
                throw new Exception($"user {request.UserId} already have referrer");
            }

            await _db.CreateEntity(new UserReferrer
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ReferrerId = request.ReferrerId,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user referrer entity for user {UserId} with referrer {ReferrerId}",
                request.UserId, request.ReferrerId);

            await _mediator.Send(new AddBoxToUserCommand(request.UserId, BoxType.Capital, 1));
            await AddRewardsToReferrer(request.UserId, request.ReferrerId);

            return Unit.Value;
        }

        private async Task AddRewardsToReferrer(long userId, long referrerId)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var referralCount = await _mediator.Send(new GetUserReferralCountQuery(referrerId));

            var rewardString = string.Empty;
            switch (referralCount)
            {
                case 1 or 2:

                    await _mediator.Send(new AddBoxToUserCommand(referrerId, BoxType.Capital, 1));

                    rewardString =
                        $"{emotes.GetEmote(BoxType.Capital.EmoteName())} {_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString())}";

                    break;

                case 3 or 4:

                    await _mediator.Send(new AddBoxToUserCommand(referrerId, BoxType.Capital, 2));

                    rewardString =
                        $"{emotes.GetEmote(BoxType.Capital.EmoteName())} 2 {_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString(), 2)}";

                    break;

                case 5:

                    await _mediator.Send(new AddBoxToUserCommand(referrerId, BoxType.Capital, 5));

                    rewardString =
                        $"{emotes.GetEmote(BoxType.Capital.EmoteName())} 5 {_local.Localize(LocalizationCategoryType.Box, BoxType.Capital.ToString(), 5)}";

                    break;

                case 6 or 7 or 8 or 9:

                    await _mediator.Send(new AddCurrencyToUserCommand(referrerId, CurrencyType.Pearl, 10));

                    rewardString =
                        $"{emotes.GetEmote(CurrencyType.Pearl.ToString())} 10 {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), 10)}";

                    break;

                case 10:

                    await _mediator.Send(new AddCurrencyToUserCommand(referrerId, CurrencyType.Pearl, 10));
                    await _mediator.Send(new AddTitleToUserCommand(referrerId, TitleType.Yatagarasu));

                    rewardString =
                        $"{emotes.GetEmote(CurrencyType.Pearl.ToString())} 10 {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), 10)}, " +
                        $"титул {emotes.GetEmote(TitleType.Yatagarasu.EmoteName())} {TitleType.Yatagarasu.Localize()}";

                    break;

                case > 10:

                    await _mediator.Send(new AddCurrencyToUserCommand(referrerId, CurrencyType.Pearl, 15));

                    rewardString =
                        $"{emotes.GetEmote(CurrencyType.Pearl.ToString())} 15 {_local.Localize(LocalizationCategoryType.Currency, CurrencyType.Pearl.ToString(), 15)}";

                    break;
            }

            var user = await _mediator.Send(new GetUserQuery(userId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery((ulong) user.Id));

            var embed = new EmbedBuilder()
                .WithAuthor("Реферальная система")
                .WithDescription(
                    $"{emotes.GetEmote(user.Title.EmoteName())} {user.Title.Localize()} {socketUser.Mention} указал тебя своим реферером и ты получаешь {rewardString}." +
                    $"\n\n{emotes.GetEmote("Arrow")} Напиши `/приглашения` чтобы посмотреть информацию о своем участии в реферальной системе.");

            await _mediator.Send(new SendEmbedToUserCommand((ulong) referrerId, embed));
        }
    }
}
