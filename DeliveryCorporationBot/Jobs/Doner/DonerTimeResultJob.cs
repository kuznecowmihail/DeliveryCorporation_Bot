using Quartz;
using System.Text;
using Telegram.Bot;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Jobs.Doner
{
    #region DonerTimeResultJob
    public class DonerTimeResultJob : IJob
    {
        #region public
        public async Task Execute(IJobExecutionContext context)
        {
            long chatId = 0;
            int messageId = 0;
            var client = BotHandler.Instance.GetClient();
            try
            {
                chatId = context.JobDetail.JobDataMap.GetLong("ChatId");
                messageId = context.JobDetail.JobDataMap.GetInt("MessageId");
                var message = await client.EditMessageReplyMarkupAsync(chatId, messageId, null);
                var participients = message.Text
                    .Split("\n")
                    .Skip(1)
                    .OrderBy(x => Guid.NewGuid().ToString())
                    .ToList();

                if (participients.Count() <= 0)
                {
                    await client.EditMessageTextAsync(chatId, messageId, $"{message.Text}\r\n{BaseConstants.NotEnoughParticipantsStr}");
                    await client.SendTextMessageAsync(chatId, BaseConstants.NotEnoughParticipantsStr);

                    return;
                }
                var pairCount = participients.Count;
                var winnerCount = new Random().Next(1, pairCount + 1);
                var resultMessage = new StringBuilder();

                for (var i = 0; i < winnerCount; i++)
                {
                    var winner = participients[i];
                    resultMessage.Append(string.Format(DonerTimeConstants.GameResultStr, winner));
                }
                await client.EditMessageTextAsync(chatId, messageId, $"{message.Text}\r\n{resultMessage.ToString()}");
                await client.SendTextMessageAsync(chatId, resultMessage.ToString());
            }
            catch (Exception ex)
            {
                Logger.Log(chatId, messageId, ex.Message);

                if (client != null && chatId != 0)
                {
                    await client.SendTextMessageAsync(chatId, ex.Message);
                }
            }
        }
        #endregion
    }
    #endregion
}
