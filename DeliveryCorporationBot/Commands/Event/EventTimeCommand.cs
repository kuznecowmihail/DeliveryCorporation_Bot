using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Event
{
    #region EventTimeCommand
    public class EventTimeCommand : Command
    {
        #region public
        public override string Name => EventTimeConstants.EventTimeCommandStr;

        public override Task Execute(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(CallbackQuery query)
        {
            var userId = query.From.Id;
            var ownerId = query?.Message?.From?.Id;
            var chatId = query?.Message?.Chat.Id ?? 0;
            var messageId = query?.Message?.MessageId ?? 0;
            var inlineQueryId = query?.Id;
            var client = BotHandler.Instance.GetClient();

            if (userId != ownerId)
            {
                //await client.AnswerCallbackQueryAsync(inlineQueryId, BaseConstants.IsntOwnerStr);

                //return;
            }
            var data = query?.Data?.Split('|');
            var value = data?.Length == 1 ? 1 : Int32.Parse(data[1]);
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("-1", $"{EventTimeConstants.EventTimeCommandStr}|{value - 1}"),
                InlineKeyboardButton.WithCallbackData(EventTimeConstants.StartEventTimeCommandDescrStr(value), $"{EventTimeConstants.StartEventTimeCommandStr}|{value}"),
                InlineKeyboardButton.WithCallbackData("+1", $"{EventTimeConstants.EventTimeCommandStr}|{value + 1}")
            });
            
            if (data.Length == 1)
            {
                await client.EditMessageTextAsync(chatId, messageId, EventTimeConstants.ChooseParticipientNumberStr, replyMarkup: keyboard);
            }
            else if (data.Length == 2)
            {
                if (value == 0)
                {
                    await client.AnswerCallbackQueryAsync(inlineQueryId, EventTimeConstants.MinParticipientNumberStr);

                    return;
                }
                await client.EditMessageReplyMarkupAsync(chatId, messageId, replyMarkup: keyboard);
            }
            
        }
        #endregion
    }
    #endregion
}
