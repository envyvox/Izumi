using System;
using System.Threading;
using System.Threading.Tasks;
using Izumi.Data.Enums;
using Izumi.Data.Util;
using Izumi.Services.Game.Contract.Commands;
using MediatR;

namespace Izumi.Services.Seeder.Game
{
    public record SeederUploadContractsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeederUploadContractsHandler : IRequestHandler<SeederUploadContractsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeederUploadContractsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeederUploadContractsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateContractCommand[]
            {
                new(LocationType.Capital, "Помощь жителям", "Требуется рабочая сила для облагораживания города.", TimeSpan.FromHours(2), 75, 2, 20),
                new(LocationType.Capital, "Помощь в торговой лавке", "Ищем людей для выкладки товаров и работе на складе.", TimeSpan.FromHours(4), 150, 5, 40),
                new(LocationType.Capital, "Помощь в администрации города", "Нужны компетентные люди для работы с документами.", TimeSpan.FromHours(8), 300, 10, 80),
                new(LocationType.Garden, "Помощь на кухне", "Требуется человек для мытья посуды.", TimeSpan.FromHours(2), 75, 2, 20),
                new(LocationType.Garden, "Уход за цветами", "Ищем трудолюбивого работника для ухода за цветами.", TimeSpan.FromHours(4), 150, 5, 40),
                new(LocationType.Garden, "Помощь в исследовании сада", "Требуется специалист по исследованию сада для помощи начинающим.", TimeSpan.FromHours(8), 300, 10, 80),
                new(LocationType.Seaport, "Разделка рыбы", "Дом рыбака ищет помощника для разделки рыбы.", TimeSpan.FromHours(2), 75, 2, 20),
                new(LocationType.Seaport, "Помощь в судостроении", "Требуется работник на пристань для помощи в судостроении.", TimeSpan.FromHours(4), 150, 5, 40),
                new(LocationType.Seaport, "Очистка прибережной зоны", "Набираем группу людей для уборки прибрежной зоны.", TimeSpan.FromHours(8), 300, 10, 80),
                new(LocationType.Castle, "Помощь в переработке ресурсов", "Требуется человек для чистки станков и рабочего места.", TimeSpan.FromHours(2), 75, 2, 20),
                new(LocationType.Castle, "Разведка территории", "Требуются бравые ребята для разведки новых территорий.", TimeSpan.FromHours(4), 150, 5, 40),
                new(LocationType.Castle, "Помощь в шахте", "Шахтеры ищут сильного помощника для раскопок шахт.", TimeSpan.FromHours(8), 300, 10, 80),
                new(LocationType.Village, "Помощь строителям", "Требуется разнорабочий на строительный объект.", TimeSpan.FromHours(2), 75, 2, 20),
                new(LocationType.Village, "Работа на участках", "Ищем людей для вспахивания земли.", TimeSpan.FromHours(4), 150, 5, 40),
                new(LocationType.Village, "Помощь в животноводстве", "На ферму нужен работник для ухода за животными.", TimeSpan.FromHours(8), 300, 10, 80)
            };

            foreach (var createContractCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createContractCommand);

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
