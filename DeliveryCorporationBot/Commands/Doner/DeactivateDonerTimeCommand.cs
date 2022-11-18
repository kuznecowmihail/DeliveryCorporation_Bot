using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text.Json;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Doner
{
    #region DeactivateDonerTimeCommand
    public class DeactivateDonerTimeCommand : Command
    {
        #region public
        public override string Name => DonerTimeConstants.DeactiveteDonerTimeCommandStr;

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
            var dataStr = await FileHandler.Instance.ReadTextAsync(DonerTimeConstants.FilePath);
            var data = JsonSerializer.Deserialize<Data>(dataStr);
            data.Chats.Remove(chatId);
            dataStr = JsonSerializer.Serialize(data);
            await FileHandler.Instance.WriteTextAsync(DonerTimeConstants.FilePath, dataStr, FileMode.Truncate);
            var keyboard = await BotHandler.Instance.GetInlineKeyboardMarkupMenu(chatId);
            await client.EditMessageReplyMarkupAsync(chatId, messageId, keyboard);
            await client.AnswerCallbackQueryAsync(inlineQueryId, DonerTimeConstants.GameDeactivatedStr);
        }
        #endregion
    }
    #endregion
}
