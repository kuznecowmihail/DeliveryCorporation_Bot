using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Jobs.Doner
{
    #region DonerTimeStartJob
    public class DonerTimeStartJob : IJob
    {
        #region public
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var client = BotHandler.Instance.GetClient();
                var keys = await FileHandler.Instance.GetKeys(DonerTimeConstants.FilePath);

                foreach (var key in keys)
                {
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                        InlineKeyboardButton.WithCallbackData(DonerTimeConstants.WantDonerCommandDescrStr, DonerTimeConstants.WantDonerCommandStr)
                    });
                    var message = await client.SendTextMessageAsync(key, DonerTimeConstants.GameStartedStr, replyMarkup: keyboard);
                    await StartJob(message);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(0, 0, ex.Message);
            }
        }

        async Task StartJob(Message message)
        {
            var date = DateTime.Now.AddMinutes(DonerTimeConstants.MinDuration);
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            var schedule = SchedularHelper.Instance.GetScheduler();
            var job = JobBuilder.Create<DonerTimeResultJob>()
                .UsingJobData("ChatId", chatId)
                .UsingJobData("MessageId", messageId)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"donerresulttrigger_{chatId}", "doner_group")
                .StartAt(date)
                .Build();
            await schedule.ScheduleJob(job, trigger);
        }
        #endregion
    }
    #endregion
}
