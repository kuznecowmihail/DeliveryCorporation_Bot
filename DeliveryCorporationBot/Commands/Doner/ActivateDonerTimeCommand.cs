using Telegram.Bot;
using Telegram.Bot.Types;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Doner
{
    #region ActivateDonerTimeCommand
    public class ActivateDonerTimeCommand : Command
    {
        #region public
        public override string Name => DonerTimeConstants.ActiveteDonerTimeCommandStr;

        public override Task Execute(Message message)
        {
            throw new NotImplementedException();
        }

        public override async Task Execute(CallbackQuery query)
        {
            var chatId = query.Message.Chat.Id;
            var messageId = query.Message.MessageId;
            var inlineQueryId = query.Id;
            var client = BotHandler.Instance.GetClient();
            await FileHandler.Instance.GetOrCreate(chatId, DonerTimeConstants.FilePath);
            var keyboard = await BotHandler.Instance.GetInlineKeyboardMarkupMenu(chatId);
            await client.EditMessageReplyMarkupAsync(chatId, messageId, keyboard);
            await client.AnswerCallbackQueryAsync(inlineQueryId, DonerTimeConstants.GameActivatedStr);
        }
        #endregion
    }
    #endregion
}
