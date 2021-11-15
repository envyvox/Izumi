﻿using System;

namespace Izumi.Data.Enums
{
    public enum TutorialStepType : byte
    {
        None = 0,
        CheckProfile = 1,
        CheckWorldInfo = 2,
        CheckTransits = 3,
        TransitToSeaport = 4,
        CompleteFishing = 5,
        CheckInventory = 6,
        TransitToGarden = 7,
        CompleteExploreGarden = 8,
        CheckCookingList = 9,
        CookFriedEgg = 10,
        EatFriedEgg = 11,
        TransitToCastle = 12,
        CompleteExploreCastle = 13,
        TransitToVillage = 14,
        CheckField = 15,
        TransitToCapital = 16,
        CheckSeedShop = 17,
        CheckLottery = 18,
        CheckContracts = 19,
        Completed = 20
    }

    public static class TutorialStepHelper
    {
        public static string Name(this TutorialStepType step) => step switch
        {
            TutorialStepType.None => "...",
            TutorialStepType.CheckProfile => "Приветствие",
            TutorialStepType.CheckWorldInfo => "Знакомство с миром",
            TutorialStepType.CheckTransits => "Время путешествовать",
            TutorialStepType.TransitToSeaport => "В путь",
            TutorialStepType.CompleteFishing => "Отличный день для рыбалки",
            TutorialStepType.CheckInventory => "Рюкзак путешественника",
            TutorialStepType.TransitToGarden => "Отправляемся в cад",
            TutorialStepType.CompleteExploreGarden => "Прогулка по cаду",
            TutorialStepType.CheckCookingList => "Восстановить силы",
            TutorialStepType.CookFriedEgg => "Кулинарный вызов",
            TutorialStepType.EatFriedEgg => "Вкусный перекус",
            TutorialStepType.TransitToCastle => "Дальнее путешествие",
            TutorialStepType.CompleteExploreCastle => "Вглубь шахт",
            TutorialStepType.TransitToVillage => "Отправляемся в деревню",
            TutorialStepType.CheckField => "Безграничные поля",
            TutorialStepType.TransitToCapital => "Возвращение в столицу",
            TutorialStepType.CheckSeedShop => "Шоппинг",
            TutorialStepType.CheckLottery => "Лотерея",
            TutorialStepType.CheckContracts => "Опять работать?",
            TutorialStepType.Completed => "До новых встреч!",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null)
        };

        public static string Description(this TutorialStepType step) => step switch
        {
            TutorialStepType.None => "",

            TutorialStepType.CheckProfile =>
                "Я тебя раньше тут не видела. Ох, где мои манеры. Привет! Меня зовут Изуми, я путешествую по миру и " +
                "завожу новых друзей. Какое-то время назад я остановилась здесь, уж очень мне тут понравилось. Расскажешь о себе?" +
                "\n\nНапиши в канал <#879838888296857630> `/профиль` для его просмотра, а если хочешь добавить " +
                "немного информации о себе, то напиши `/информация [текст]`.",

            TutorialStepType.CheckWorldInfo =>
                "Приятно познакомиться! Я всегда готова помочь своим новым знакомым, поэтому проведу тебе небольшую экскурсию!" +
                "\n\nСейчас мы находимся в **столице «Эдо»**, можешь почитать об ее истории в канале <#750624440391434321>. " +
                "Но начнем мы не с неё. Для начала дам тебе один совет." +
                "\n\nНапиши в канал <#879838888296857630> `/мир`, чтобы узнать информацию о погоде, времени суток и прочее. " +
                "Это пригодится тебе в дальнейшем.",

            TutorialStepType.CheckTransits =>
                "Отлично, а теперь за дело!" +
                "\n\nЕсли ты тут хочешь задержаться, то тебе понадобятся <:Ien:813145486428995635> деньги. Нужно что-то придумать..." +
                "\n\nНапиши в канал <#879838888296857630> `/отправления`.",

            TutorialStepType.TransitToSeaport =>
                "О, кажется у меня есть идея!" +
                "\nПоехали в **портовый город «Нагоя»**!" +
                "\n\nНапиши в канал <#879838888296857630> `/отправиться` и выбери **портовый город** из списка вариантов.",

            TutorialStepType.CompleteFishing =>
                "Ох, обязательно прочитай историю об этом городе в канале <#750624448004227113>, она такая захватывающая!" +
                "\n\nВ **портовом городе «Нагоя»** кланы и семьи отправляются в исследования на своих кораблях, ну, а " +
                "нам новичкам, чтобы заработать <:Ien:813145486428995635> денег остается только рыбачить." +
                "\n\nНапиши в канал <#879838888296857630> `/рыбачить`.",


            TutorialStepType.CheckInventory =>
                "По рассказам жителей тут водится огромное количество видов рыб, и от ее редкости зависит цена, которую дает местный рыбак." +
                "\n\nНо это еще не все, рыбалка не такое простое дело. Некоторых рыб нужно вылавливать в определенную погоду и время суток." +
                "\n\nА ты сколько поймал? Напиши в канал <#879838888296857630> `/инвентарь`, а если же хочешь узнать " +
                "более подробную информацию то в меню команды выбери категорию **рыба** из списка вариантов.",

            TutorialStepType.TransitToGarden =>
                "\n\nДа уж, что-то мне сегодня не везет с уловом. Ну, а ты если что-то поймал, то можешь продать рыбаку." +
                "\n\nЦены на рыбку ты можешь посмотреть с помощью команды `/рыбак`, там же он тебе расскажет как ее продать." +
                "Такс, куда мы отправимся теперь... Точно! Я была в **цветущем саду «Кайраку-Эн»**, там так красиво! Тебе обязательно нужно там побывать." +
                "\n\nНапиши в канал <#879838888296857630> `/отправиться` и выбери **цветущий сад** из списка вариантов.",

            TutorialStepType.CompleteExploreGarden =>
                "Помимо красоты тут есть еще и полезные ресурсы, тебе же надо будет возводить собственные постройки или ты настолько любишь природу?" +
                "\n\nКстати, на эти ресурсы в столице бывает спрос, так что стоит собрать побольше." +
                "\n\nНапиши в канал <#879838888296857630> `/исследовать`." +
                "\n\nА пока прочитай <#750624444078227497> **цветущего сада «Кайраку-Эн»**, оно просто волшебное!",

            TutorialStepType.CheckCookingList =>
                "После тяжелой работы необходимо восстановить <:Energy:835868948561002546> энергию, ведь именно от ее " +
                "количества зависит как быстро ты будешь действовать. Чем меньше <:Energy:835868948561002546> энергии - " +
                "тем больше времени тебе понадобиться чтобы выполнить действие." +
                "\n\nНапиши в канал <#879838888296857630> `/приготовление`.",

            TutorialStepType.CookFriedEgg =>
                "Тут собраны все рецепты блюд, а сейчас самое время приготовить <:FriedEgg:813146110612340747> яичницу!" +
                "\n\nНа этот раз я подарю тебе <:Recipe:813150911530401844> рецепт из категории начинающего повара и " +
                "<:Egg:813145655316971581> необходимые ингредиенты, однако в будущем тебе придется покупать " +
                "<:Recipe:813150911530401844> рецепты и добывать все ингредиенты самостоятельно." +
                "\n\nА теперь напиши в канал <#879838888296857630> `/приготовить 1 яичница`.",

            TutorialStepType.EatFriedEgg =>
                "А у тебя неплохо получилось для первого раза. Время перекусить и восстановить <:Energy:835868948561002546> энергию." +
                "\n\nНапиши в канал <#879838888296857630> `/съесть 1 яичница`.",

            TutorialStepType.TransitToCastle =>
                "Количество <:Energy:835868948561002546> энергии определяет скорость с которой ты выполняешь действия. " +
                "Ты можешь продолжать свое путешествие даже при <:Energy:835868948561002546> 0 энергии, " +
                "однако чем больше <:Energy:835868948561002546> энергии - тем быстрее ты справишься." +
                "\n\nНа первое время я выдам тебе <:SpecialPumpkinPie:909535318733107251> 30 особых тыквенных пирогов. Не забывай кушать!" +
                "\n\nПосле **цветущего сада «Кайраку-Эн»** мы с тобой отправимся в не менее загадочное место. " +
                "Я говорю о **древнем замке «Химэдзи»**. Ох, мне уже жутко!" +
                "\n\nНапиши в канал <#879838888296857630> `/отправиться` и выбери **древний замок** из списка вариантов.",

            TutorialStepType.CompleteExploreCastle =>
                "Запомни, мы сюда приехали только за ресурсами!" +
                "\n\nНапиши в канал <#879838888296857630> `/копать`." +
                "\n\nА пока можешь почитать <#750624451640557638> **древнего замка «Химэдзи»**, лично у меня аж мурашки пробежали!",

            TutorialStepType.TransitToVillage =>
                "*Устало села на пенёк*" +
                "\nА ты неплохо справился. Ну у меня просто совершенно другие таланты! Я... Неплохо танцую, например!" +
                "\n\nО чем это я, ах да, теперь нам нужно отправиться в живописную **деревушку Мура**. Поехали?" +
                "\n\nНапиши в канал <#879838888296857630> `/отправиться` и выбери **деревню** из списка вариантов.",

            TutorialStepType.CheckField =>
                "Давай немного расскажу зачем тебе вообще нужны ресурсы." +
                "\n\nВо-первых, это еще один небольшой способ подзаработать, но об этом позже." +
                "\n\nВо-вторых, ресурсы понадобятся тебе для различных построек, но сначала их нужно переработать и..." +
                "\n\nОй, смотри какие пейзажи! Это значит, что мы уже на месте. Кстати, наверняка ты уже понял, " +
                "где можно почитать про деревню. В **деревне «Мура»** есть много свободных участков, " +
                "один их них ты можешь купить и выращивать урожай!" +
                "\n\nНапиши в канал <#879838888296857630> `/участок информация`.",

            TutorialStepType.TransitToCapital =>
                "Так-с, здесь мы были, там побывали." +
                "\n\nНу что же, пора возвращаться в **столицу «Эдо»** - самый людный город, не зря же это столица." +
                "\n\nНапиши в канал <#879838888296857630> `/отправиться` и выбери **столицу** из списка вариантов.",

            TutorialStepType.CheckSeedShop =>
                "Давай пройдемся по городу." +
                "\nДля начала мы заглянем к **Торедо**." +
                "\n\nНапиши в канал <#879838888296857630> `/магазин-посмотреть` и выбери **семена** из списка вариантов.",

            TutorialStepType.CheckLottery =>
                "Здесь продаются семена посезонно, но всегда есть из чего выбрать. Можешь сам убедиться." +
                "\n\nГлаза разбегаются, пошли скорее отсюда, пока я все <:Ien:813145486428995635> деньги тут не оставила!" +
                "\n\nЗаглянув в `/рынок` ты можешь как купить нужные ресурсы и урожай, так и продать ненужные." +
                "\n\nА теперь я покажу тебе самое любимое место для людей, которые любят азарт, в том числе и меня!" +
                "\n\nУже догадался? Да, это **казино**." +
                "\n\nНапиши в канал <#879838888296857630> `/лотерея информация`.",

            TutorialStepType.CheckContracts =>
                "\n\nЗдесь проводится <:LotteryTicket:813150225597202474> лотерея, где у тебя есть шанс сорвать <:Ien:813145486428995635> куш" +
                ".\n\nА если хочешь испытать свою удачу прямо сейчас, то напиши `/ставка` и введи сумму которую хочешь поставить. " +
                "\n\nНу все, пойдем скорее на воздух, а то у меня начинает кружиться голова." +
                "\n\nЕсли у тебя наступили <:Ien:813145486428995635> финансовые трудности, то в любом городе жители " +
                "могут предложить тебе небольшую работу с соответствующей оплатой." +
                "\n\nНапиши в канал <#879838888296857630> `/контракты`.",

            TutorialStepType.Completed =>
                "Ну, что же, друг, я показала тебе все что нужно, чтобы ты смог обустроиться здесь. " +
                "Держи <:Ien:813145486428995635> 1000 иен, потрать их с умом!" +
                "\n\nТак же, с этого момента ты можешь написать в канал <#879838888296857630> `/ежедневная-награда` " +
                "чтобы получать немного <:Ien:813145486428995635> валюты каждый день. Далее дело за тобой, но я всегда буду на связи." +
                "\n\nИ еще кое-что! У меня есть <#900148533355753512>, где я буду рассказывать обо всем интересном, " +
                "что будет происходить в мире **Hinode**, не забывай читать!",


            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null)
        };
    }
}
