using Telegram.Bot.Types;

namespace DeliveryCorporationBot.Commands
{
    #region Command
    public abstract class Command
    {
        #region public
        public abstract string Name { get; }
        public abstract Task Execute(Message message);
        public abstract Task Execute(CallbackQuery query);
        public bool Contains(string command) => !string.IsNullOrEmpty(command) && command.Contains(Name);
        #endregion
    }
    #endregion
}
