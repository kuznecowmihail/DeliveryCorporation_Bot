using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Commands.Coffee
{
    #region DeactivateCoffeeTimeCommand
    public class DeactivateCoffeeTimeCommand : Command
    {
        #region public
        public override string Name => CoffeeTimeConstants.DeactiveteCoffeeTimeCommandStr;

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
            var dataStr = await FileHandler.Instance.ReadTextAsync(CoffeeTimeConstants.FilePath);
            var data = JsonSerializer.Deserialize<Data>(dataStr);
            data.Chats.Remove(chatId);
            dataStr = JsonSerializer.Serialize(data);
            await FileHandler.Instance.WriteTextAsync(CoffeeTimeConstants.FilePath, dataStr, FileMode.Truncate);
            var keyboard = await BotHandler.Instance.GetInlineKeyboardMarkupMenu(chatId);
            await client.EditMessageReplyMarkupAsync(chatId, messageId, keyboard);
            await client.AnswerCallbackQueryAsync(inlineQueryId, CoffeeTimeConstants.GameDeactivatedStr);
        }
        #endregion
    }
    #endregion
}
