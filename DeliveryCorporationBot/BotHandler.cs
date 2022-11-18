using Quartz;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using DeliveryCorporationBot.Jobs.Doner;
using DeliveryCorporationBot.Jobs.Coffee;
using DeliveryCorporationBot.Constants;
using DeliveryCorporationBot.Commands;
using DeliveryCorporationBot.Commands.Coffee;
using DeliveryCorporationBot.Commands.Doner;
using DeliveryCorporationBot.Commands.Event;

namespace DeliveryCorporationBot
{
    #region GetterBot
    public class BotHandler
    {
        #region static
        static BotHandler _instance;
        public static BotHandler Instance => _instance ?? (_instance = new BotHandler());
        #endregion
        #region private
        readonly TelegramBotClient _client;

        BotHandler()
        {
            using var _source = new CancellationTokenSource();
            _client = new TelegramBotClient(AppSettings.Key);
            _client.StartReceiving(HandleUpdateAsync, PollingErrorHandler, null, _source.Token);
            _client.SetMyCommandsAsync(GetBotCommands());
            CoffeeTimeScheduleStartAsync().Wait();
            DonerTimeScheduleStart().Wait();
        }

        List<Command> GetMessageCommands() => new List<Command>()
        {
            new MenuCommand()
        };

        List<Command> GetInlineCommands() => new List<Command>()
        {
            new ActivateCoffeeTimeCommand(),
            new DeactivateCoffeeTimeCommand(),
            new WantCoffeeCommand(),
            new ActivateDonerTimeCommand(),
            new DeactivateDonerTimeCommand(),
            new WantDonerCommand(),
            new EventTimeCommand(),
            new StartEventTimeCommand(),
            new WantEventTimeCommand(),
            new FinishEventTimeCommand()
        };

        List<BotCommand> GetBotCommands() => new List<BotCommand>()
        {
            new BotCommand()
            {
                Command = BaseConstants.MenuCommandStr,
                Description = BaseConstants.MenuCommandDescrStr
            }
        };

        Task PollingErrorHandler(ITelegramBotClient bot, Exception ex, CancellationToken ct)
        {
            Console.WriteLine($"Exception while polling for updates: {ex}");
            return Task.CompletedTask;
        }

        async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            try
            {
                await (update.Type switch
                {
                    UpdateType.Message => BotOnMessageReceived(update.Message),
                    UpdateType.CallbackQuery => BotOnCallbackReceived(update.CallbackQuery),
                    _ => Task.CompletedTask
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-Exception while handling {update.Type}: {ex}");
                await (update.Type switch
                {
                    UpdateType.Message => client.SendTextMessageAsync(update.Message.Chat.Id, ex.Message),
                    UpdateType.CallbackQuery => client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, ex.Message),
                    _ => Task.CompletedTask
                });
            }
        }

        async Task BotOnMessageReceived(Message message) =>
            await GetMessageCommands()
                .First(command => command.Contains(message.Text))
                .Execute(message);

        async Task BotOnCallbackReceived(CallbackQuery query) =>
            await GetInlineCommands()
                .First(command => command.Contains(query.Data))
                .Execute(query);

        async Task CoffeeTimeScheduleStartAsync()
        {
            DateTime nextDayOfWeek;
            var today11Am = DateTime.Today.AddHours(CoffeeTimeConstants.StartHour).AddMinutes(CoffeeTimeConstants.StartMinute);

            if (DateTime.Now.DayOfWeek == CoffeeTimeConstants.DayOfWeek && DateTime.Now < today11Am)
            {
                nextDayOfWeek = today11Am;
            }
            else
            {
                int daysUntilTuesday = ((int)CoffeeTimeConstants.DayOfWeek - (int)today11Am.DayOfWeek + 7) % 7;
                nextDayOfWeek = today11Am.AddDays(daysUntilTuesday);
            }
            var job = JobBuilder.Create<CoffeeTimeStartJob>().Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("coffeetrigger", "coffee_group")
                .StartAt(nextDayOfWeek)
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(168)
                    .RepeatForever())
                .Build();
            await SchedularHelper.Instance.GetScheduler().ScheduleJob(job, trigger);
        }

        async Task DonerTimeScheduleStart()
        {
            DateTime nextDayOfWeek;
            var today11Am = DateTime.Today.AddHours(DonerTimeConstants.StartHour).AddMinutes(DonerTimeConstants.StartMinute);

            if (DateTime.Now.DayOfWeek == DonerTimeConstants.DayOfWeek && DateTime.Now < today11Am)
            {
                nextDayOfWeek = today11Am;
            }
            else
            {
                int daysUntilTuesday = ((int)DonerTimeConstants.DayOfWeek - (int)today11Am.DayOfWeek + 7) % 7;
                nextDayOfWeek = today11Am.AddDays(daysUntilTuesday);
            }
            var job = JobBuilder.Create<DonerTimeStartJob>().Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("donertrigger", "doner_group")
                .StartAt(nextDayOfWeek)
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(168)
                    .RepeatForever())
                .Build();
            await SchedularHelper.Instance.GetScheduler().ScheduleJob(job, trigger);
        }
        #endregion
        #region public
        public ITelegramBotClient GetClient() => _client;

        public async Task<InlineKeyboardMarkup> GetInlineKeyboardMarkupMenu(long chatId)
        {
            var coffeeDataStr = await FileHandler.Instance.ReadTextAsync(CoffeeTimeConstants.FilePath);
            var coffeeData = JsonSerializer.Deserialize<Data>(coffeeDataStr);
            var donerDataStr = await FileHandler.Instance.ReadTextAsync(DonerTimeConstants.FilePath);
            var donerData = JsonSerializer.Deserialize<Data>(donerDataStr);
            var isCoffeeActive = coffeeData?.Chats.Contains(chatId) ?? false;
            var isDonerActive = donerData?.Chats.Contains(chatId) ?? false;
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(
                        isCoffeeActive
                        ? CoffeeTimeConstants.DeactiveteCoffeeTimeCommandDescrStr
                        : CoffeeTimeConstants.ActiveteCoffeeTimeCommandDescrStr,
                        isCoffeeActive
                        ? CoffeeTimeConstants.DeactiveteCoffeeTimeCommandStr
                        : CoffeeTimeConstants.ActiveteCoffeeTimeCommandStr)
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(
                        isDonerActive
                        ? DonerTimeConstants.DeactiveteDonerTimeCommandDescrStr
                        : DonerTimeConstants.ActiveteDonerTimeCommandDescrStr,
                        isDonerActive
                        ? DonerTimeConstants.DeactiveteDonerTimeCommandStr
                        : DonerTimeConstants.ActiveteDonerTimeCommandStr)
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(EventTimeConstants.EventTimeCommandDescrStr, EventTimeConstants.EventTimeCommandStr)
                }
            });

            return keyboard;
        }
        #endregion
    }
    #endregion
}
