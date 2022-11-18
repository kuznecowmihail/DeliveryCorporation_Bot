using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Jobs.Coffee
{
    #region CoffeeTimeStartJob
    public class CoffeeTimeStartJob : IJob
    {
        #region public
        public async Task Execute(IJobExecutionContext context)
        {
            var client = BotHandler.Instance.GetClient();
            try
            {
                var chatIDs = await FileHandler.Instance.GetKeys(CoffeeTimeConstants.FilePath);

                foreach (var chatId in chatIDs)
                {
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                        InlineKeyboardButton.WithCallbackData(CoffeeTimeConstants.WantCoffeeCommandDescrStr, CoffeeTimeConstants.WantCoffeeCommandStr)
                    });
                    var message = await client.SendTextMessageAsync(chatId, CoffeeTimeConstants.GameStartedStr, replyMarkup: keyboard);
                    await StartJob(message);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(0, 0, ex.Message);
            }
        }

        async Task StartJob(Message message)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            var date = DateTime.Now.AddMinutes(CoffeeTimeConstants.MinDuration);
            var schedule = SchedularHelper.Instance.GetScheduler();
            var job = JobBuilder.Create<CoffeeTimeResultJob>()
                .UsingJobData("ChatId", chatId)
                .UsingJobData("MessageId", messageId)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"coffeetimeresulttrigger_{chatId}", "coffee_group")
                .StartAt(date)
                .Build();
            await schedule.ScheduleJob(job, trigger);
        }
        #endregion
    }
    #endregion
}
