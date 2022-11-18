using Telegram.Bot;
using DeliveryCorporationBot;

// start the bot
var bot = BotHandler.Instance.GetClient();
var me = await bot.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// stop the bot
//bot.Item2.Cancel();