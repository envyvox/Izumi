using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Discord.Guild.Queries;
using Izumi.Services.Game.Banner.Commands;
using MediatR;
using Microsoft.Extensions.Options;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadBannersCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadBannersHandler : IRequestHandler<SeederUploadBannersCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;
        private readonly IOptions<DiscordOptions> _options;

        public SeederUploadBannersHandler(
            IMediator mediator,
            IOptions<DiscordOptions> options)
        {
            _mediator = mediator;
            _options = options;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadBannersCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new List<CreateBannerCommand>();

            var guild = await _mediator.Send(new GetSocketGuildQuery(_options.Value.FilesGuildId));
            var bannerRarities = Enum
                .GetValues(typeof(BannerRarityType))
                .Cast<BannerRarityType>();

            foreach (var rarity in bannerRarities)
            {
                var channel = guild.TextChannels.First(x => x.Name == "banner-" + rarity.ToString().ToLower());
                var messages = await channel.GetMessagesAsync().FlattenAsync();

                commands.AddRange(messages.Select(message => new CreateBannerCommand(
                    message.Content.Replace("`", ""), rarity, 9999, message.Attachments.First().Url)));
            }

            foreach (var createBannerCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createBannerCommand);

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}
