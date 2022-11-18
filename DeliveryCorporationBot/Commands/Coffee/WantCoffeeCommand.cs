using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Coffee
{
    #region WantCoffeeCommand
    public class WantCoffeeCommand : Command
    {
        #region public
        public override string Name => CoffeeTimeConstants.WantCoffeeCommandStr;

        public override Task Execute(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(CallbackQuery query)
        {
            var chatId = query.Message.Chat.Id;
            var messageId = query?.Message?.MessageId ?? 0;
            var from = query.From;
            var inlineQueryId = query.Id;
            var client = BotHandler.Instance.GetClient();
            var startTime = new TimeSpan(CoffeeTimeConstants.StartHour, CoffeeTimeConstants.StartMinute, 0);
            var finishTime = new TimeSpan(CoffeeTimeConstants.StartHour, CoffeeTimeConstants.StartMinute + CoffeeTimeConstants.MinDuration, 0);

            if (!await FileHandler.Instance.ContainsKey(chatId, CoffeeTimeConstants.FilePath))
            {
                var keyboard = new InlineKeyboardMarkup(
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(CoffeeTimeConstants.ActiveteCoffeeTimeCommandDescrStr, CoffeeTimeConstants.ActiveteCoffeeTimeCommandStr)
                    });
                await client.SendTextMessageAsync(chatId, CoffeeTimeConstants.GameDidntActivateStr, replyMarkup: keyboard);
            }
            else if (DateTime.Now.DayOfWeek == CoffeeTimeConstants.DayOfWeek
                && DateTime.Now.TimeOfDay >= startTime
                && DateTime.Now.TimeOfDay < finishTime)
            {
                var participient = !string.IsNullOrWhiteSpace(from.Username)
                    ? $"@{from.Username}"
                    : $"tg://user?id={from.Id}";
                var participients = query.Message.Text
                    .Split("\n")
                    .Skip(1)
                    .OrderBy(x => Guid.NewGuid().ToString())
                    .ToList();

                if (participients.Contains(participient))
                {
                    await client.AnswerCallbackQueryAsync(inlineQueryId, BaseConstants.YouAreAlreadyParticipantStr);

                    return;
                }
                var message = query.Message.Text + $"\r\n{participient}";
                var keyborad = query.Message.ReplyMarkup;
                await client.EditMessageTextAsync(chatId, messageId, message, replyMarkup: keyborad);
                await client.AnswerCallbackQueryAsync(inlineQueryId, BaseConstants.YouAreParticipantStr);
            }
            else
            {
                await client.AnswerCallbackQueryAsync(inlineQueryId, CoffeeTimeConstants.NotGameTimeStr);
            }
        }
        #endregion
    }
    #endregion
}
