using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Alcohol.Queries;
using Izumi.Services.Game.Crafting.Queries;
using Izumi.Services.Game.Crop.Queries;
using Izumi.Services.Game.Drink.Queries;
using Izumi.Services.Game.Fish.Queries;
using Izumi.Services.Game.Food.Queries;
using Izumi.Services.Game.Gathering.Queries;
using Izumi.Services.Game.Localization.Commands;
using Izumi.Services.Game.Product.Queries;
using Izumi.Services.Game.Seafood.Queries;
using Izumi.Services.Game.Seed.Queries;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadLocalizationsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadLocalizationsHandler
        : IRequestHandler<SeederUploadLocalizationsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadLocalizationsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadLocalizationsCommand request,
            CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new List<CreateLocalizationCommand>();

            foreach (var category in Enum.GetValues(typeof(LocalizationCategoryType)).Cast<LocalizationCategoryType>())
            {
                switch (category)
                {
                    case LocalizationCategoryType.Gathering:
                    {
                        var entities = await _mediator.Send(new GetGatheringsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }

                    case LocalizationCategoryType.Product:
                    {
                        var entities = await _mediator.Send(new GetProductsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Crafting:
                    {
                        var entities = await _mediator.Send(new GetCraftingsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Alcohol:
                    {
                        var entities = await _mediator.Send(new GetAlcoholsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Drink:
                    {
                        var entities = await _mediator.Send(new GetDrinksQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Seed:
                    {
                        var entities = await _mediator.Send(new GetSeedsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Crop:
                    {
                        var entities = await _mediator.Send(new GetCropsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Fish:
                    {
                        var entities = await _mediator.Send(new GetFishesQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Food:
                    {
                        var entities = await _mediator.Send(new GetFoodsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Currency:
                    {
                        var currencies = Enum.GetValues(typeof(CurrencyType)).Cast<CurrencyType>();

                        commands.AddRange(currencies.Select(currency => new CreateLocalizationCommand(
                            category, currency.ToString(), currency.ToString(), currency.ToString(),
                            currency.ToString())));

                        break;
                    }
                    case LocalizationCategoryType.Bar:
                    {
                        commands.Add(new CreateLocalizationCommand(
                            category, "Energy", "энергия", "энергии", "энергии"));

                        break;
                    }
                    case LocalizationCategoryType.Box:
                    {
                        var boxes = Enum.GetValues(typeof(BoxType)).Cast<BoxType>();

                        commands.AddRange(boxes.Select(currency => new CreateLocalizationCommand(
                            category, currency.ToString(), currency.ToString(), currency.ToString(),
                            currency.ToString())));

                        break;
                    }
                    case LocalizationCategoryType.Points:
                    {
                        break;
                    }
                    case LocalizationCategoryType.Seafood:
                    {
                        var entities = await _mediator.Send(new GetSeafoodsQuery());

                        commands.AddRange(entities.Select(entity => new CreateLocalizationCommand(
                            category, entity.Name, entity.Name, entity.Name, entity.Name)));

                        break;
                    }
                    case LocalizationCategoryType.Event:
                    {
                        break;
                    }
                    case LocalizationCategoryType.Vote:
                    {
                        commands.Add(new CreateLocalizationCommand(
                            category, "Like", "лайк", "лайка", "лайков"));
                        commands.Add(new CreateLocalizationCommand(
                            category, "Dislike", "дизлайк", "дизлайка", "дизлайков"));

                        break;
                    }
                    case LocalizationCategoryType.Basic:
                    {
                        commands.Add(new CreateLocalizationCommand(
                            category, "User", "пользователь", "пользователя", "пользователей"));
                        commands.Add(new CreateLocalizationCommand(
                            category, "Message", "сообщение", "сообщения", "сообщений"));

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var command in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(command);

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
