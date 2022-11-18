using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Doner
{
    #region WantDonerCommand
    public class WantDonerCommand : Command
    {
        #region public
        public override string Name => DonerTimeConstants.WantDonerCommandStr;

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
            var startTime = new TimeSpan(DonerTimeConstants.StartHour, DonerTimeConstants.StartMinute, 0);
            var finishTime = new TimeSpan(DonerTimeConstants.StartHour, DonerTimeConstants.StartMinute + DonerTimeConstants.MinDuration, 0);

            if (!await FileHandler.Instance.ContainsKey(chatId, DonerTimeConstants.FilePath))
            {
                var keyboard = new InlineKeyboardMarkup(
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(BaseConstants.ActivateStr, "activatedonertime")
                    });
                await client.SendTextMessageAsync(chatId, DonerTimeConstants.GameDidntActivateStr, replyMarkup: keyboard);
            }
            else if (DateTime.Now.DayOfWeek == DonerTimeConstants.DayOfWeek
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
                await client.AnswerCallbackQueryAsync(inlineQueryId, DonerTimeConstants.NotGameTimeStr);
            }
        }
        #endregion
    }
    #endregion
}
