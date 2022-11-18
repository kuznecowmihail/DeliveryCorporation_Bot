using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Event
{
    #region StartEventTimeCommand
    public class StartEventTimeCommand : Command
    {
        #region public
        public override string Name => EventTimeConstants.StartEventTimeCommandStr;

        public override Task Execute(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(CallbackQuery query)
        {
            var chatId = query?.Message?.Chat.Id ?? 0;
            var messageId = query?.Message?.MessageId ?? 0;
            var client = BotHandler.Instance.GetClient();
            var data = query?.Data?.Split('|');
            var value = data?.Length == 1 ? 1 : Int32.Parse(data[1]);
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData(BaseConstants.ActivateStr, EventTimeConstants.WantEventTimeCommandStr),
                InlineKeyboardButton.WithCallbackData(EventTimeConstants.FinishEventTimeCommandDescrStr, $"{EventTimeConstants.FinishEventTimeCommandStr}|{value}")
            });
            await client.EditMessageTextAsync(chatId, messageId, EventTimeConstants.StartedEventTime, replyMarkup: keyboard);
        }
        #endregion
    }
    #endregion
}
