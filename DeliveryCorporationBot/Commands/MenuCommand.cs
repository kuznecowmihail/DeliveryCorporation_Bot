using DeliveryCorporationBot.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DeliveryCorporationBot.Commands
{
    #region MenuCommand
    public class MenuCommand : Command
    {
        #region public
        public override string Name => BaseConstants.MenuCommandStr;

        public override async Task Execute(Message message)
        {
            var client = BotHandler.Instance.GetClient();
            var chatId = message.Chat.Id;
            var keyboard = await BotHandler.Instance.GetInlineKeyboardMarkupMenu(chatId);
            await client.SendTextMessageAsync(chatId, BaseConstants.MenuCommandDescrStr, replyMarkup: keyboard);
        }

        public override Task Execute(CallbackQuery query)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion
}
