namespace DeliveryCorporationBot.Constants
{
    #region CoffeeTimeConstants
    public static class CoffeeTimeConstants
    {
        #region static path
        public static string FilePath { get; } = $"..\\..\\..\\Data\\CoffeeChats.json";
        #endregion
        #region static duration
        public static DayOfWeek DayOfWeek { get; } = DayOfWeek.Monday;
        public static int StartHour { get; } = 10;
        public static int StartMinute { get; } = 0;
        public static int MinDuration { get; } = 60;
        #endregion
        #region static commands
        public static string ActiveteCoffeeTimeCommandStr { get; } = "activatecoffeetime";
        public static string ActiveteCoffeeTimeCommandDescrStr { get; } = "Активировать 'Время кофе'";
        public static string WantCoffeeCommandStr { get; } = "wantcoffee";
        public static string WantCoffeeCommandDescrStr { get; } = "Хочу кофе🙈";
        public static string DeactiveteCoffeeTimeCommandStr { get; } = "dectivatecoffeetime";
        public static string DeactiveteCoffeeTimeCommandDescrStr { get; } = "Диактивировать 'Время кофе'";
        #endregion
        #region static strings
        public static string GameActivatedStr { get; } = $"Игра 'Время кофе' активирована!\r\nКаждый {DayOfWeek} в {StartHour.ToString().PadLeft(2, '0')}:{StartMinute.ToString().PadLeft(2, '0')}.";
        public static string GameDeactivatedStr { get; } = "Игра 'Время кофе' деактивирована";
        public static string GameResultStr { get; } = "{0} получает кофе от {1} (жди доставку до рабочего места)\r\n";
        public static string GameStartedStr { get; } = $"Игра 'Время кофе' началась! Хочешь испытать удачу и получить готовый кофе от коллеги? Учавствуй! Через {MinDuration} минут подводим итоги! Участники:";
        public static string GameDidntActivateStr { get; } = "Вы не активировали игру 'Время кофе'!";
        public static string NotGameTimeStr { get; } = $"Еще не 'Время кофе'!\r\nКаждый {DayOfWeek} в {StartHour.ToString().PadLeft(2, '0')}:{StartMinute.ToString().PadLeft(2, '0')}.";
        #endregion
    }
    #endregion
}
