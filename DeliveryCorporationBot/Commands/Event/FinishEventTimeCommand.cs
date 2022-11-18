using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Event
{
    #region FinishEventTimeCommand
    public class FinishEventTimeCommand : Command
    {
        #region public
        public override string Name => EventTimeConstants.FinishEventTimeCommandStr;

        public override Task Execute(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(CallbackQuery query)
        {
            var chatId = query?.Message?.Chat.Id ?? 0;
            var messageId = query?.Message?.MessageId ?? 0;
            var inlineQueryId = query.Id;
            var client = BotHandler.Instance.GetClient();
            var data = query?.Data?.Split('|');
            var value = data?.Length == 1 ? 1 : Int32.Parse(data[1]);
            var participients = query.Message.Text
                .Split("\n")
                .Skip(1)
                .OrderBy(x => Guid.NewGuid().ToString())
                .ToList();

            if(participients.Count() < value)
            {
                await client.AnswerCallbackQueryAsync(inlineQueryId, BaseConstants.NotEnoughParticipantsStr);

                return;
            }
            participients = participients.Take(value).ToList();
            var resultMessage = new StringBuilder(EventTimeConstants.ResultStr);

            foreach(var participient in participients)
            {
                resultMessage.Append($"\r\n{participient}");
            }
            await client.EditMessageTextAsync(chatId, messageId, resultMessage.ToString());
            await client.SendTextMessageAsync(chatId, resultMessage.ToString());
        }
        #endregion
    }
    #endregion
}
