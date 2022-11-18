using Telegram.Bot;
using Telegram.Bot.Types;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Event
{
    #region WantEventTimeCommand
    public class WantEventTimeCommand : Command
    {
        #region public
        public override string Name => EventTimeConstants.WantEventTimeCommandStr;

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
            var user = query.From;
            var participient = !string.IsNullOrWhiteSpace(user.Username)
                ? $"@{user.Username}"
                : $"tg://user?id={user.Id}";
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
            await client.EditMessageTextAsync(chatId, messageId, message, replyMarkup:keyborad);
        }
        #endregion
    }
#endregion
}
