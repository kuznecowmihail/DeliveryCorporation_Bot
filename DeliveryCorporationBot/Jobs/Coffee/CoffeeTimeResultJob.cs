using Quartz;
using System.Text;
using Telegram.Bot;
using DeliveryCorporationBot.Constants;

namespace DeliveryCorporationBot.Jobs.Coffee
{
    #region CoffeeTimeResultJob
    public class CoffeeTimeResultJob : IJob
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

                if (participients.Count() < 2)
                {
                    await client.EditMessageTextAsync(chatId, messageId, $"{message.Text}\r\n{BaseConstants.NotEnoughParticipantsStr}");
                    await client.SendTextMessageAsync(chatId, BaseConstants.NotEnoughParticipantsStr);

                    return;
                }
                var pairCount = participients.Count / 2;
                var winnerCount = new Random().Next(1, pairCount + 1);
                var resultMessage = new StringBuilder();

                for (var i = 0; i < winnerCount; i++)
                {
                    var winner = participients[i * 2];
                    var loser = participients[i * 2 + 1];
                    resultMessage.Append(string.Format(CoffeeTimeConstants.GameResultStr, winner, loser));
                }
                await client.EditMessageTextAsync(chatId, messageId, $"{message.Text}\r\n{resultMessage.ToString()}");
                await client.SendTextMessageAsync(chatId, resultMessage.ToString());
            }
            catch(Exception ex)
            {
                Logger.Log(chatId, messageId, ex.Message);

                if(client != null && chatId != 0)
                {
                    await client.SendTextMessageAsync(chatId, ex.Message);
                }
            }
            
        }
        #endregion
    }
    #endregion
}
