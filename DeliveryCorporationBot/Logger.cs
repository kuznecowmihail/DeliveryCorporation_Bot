namespace DeliveryCorporationBot
{
    public static class Logger
    {
        public static void Log(long chatId, int messageId, string text)
        {
            Console.WriteLine($"chatId:{chatId},messageId:{messageId},text:{text}");
        }
    }
}
