namespace DeliveryCorporationBot.Constants
{
    #region DonerTimeConstants
    public static class DonerTimeConstants
    {
        #region static path
        public static string FilePath => $"..\\..\\..\\Data\\DonerChats.json";
        #endregion
        #region static duration
        public static DayOfWeek DayOfWeek { get; } = DayOfWeek.Friday;
        public static int StartHour { get; } = 11;
        public static int StartMinute { get; } = 0;
        public static int MinDuration { get; } = 30;
        #endregion
        #region static commands
        public static string ActiveteDonerTimeCommandStr { get; } = "activatedonertime";
        public static string ActiveteDonerTimeCommandDescrStr { get; } = "Активировать 'Время шаурмы'";
        public static string WantDonerCommandStr { get; } = "wantdoner";
        public static string WantDonerCommandDescrStr { get; } = "Хочу шаурму☄️";
        public static string DeactiveteDonerTimeCommandStr { get; } = "dectivatedonertime";
        public static string DeactiveteDonerTimeCommandDescrStr { get; } = "Диактивировать 'Время шаурмы'";
        #endregion
        #region static strings
        public static string GameActivatedStr { get; } = $"Игра 'Время шаурмы' активирована!\r\nКаждый {DayOfWeek} в {StartHour.ToString().PadLeft(2, '0')}:{StartMinute.ToString().PadLeft(2, '0')}.";
        public static string GameDeactivatedStr { get; } = "Игра 'Время шаурмы' деактивирована";
        public static string GameResultStr { get; } = "{0} выйграл(а) путешествие за шаурмой\r\n";
        public static string GameStartedStr { get; } = $"Игра 'Время шаурмы' началась! Хочешь испытать удачу и помочь привезти обед коллегам? Учавствуй! Через {MinDuration} минут подводим итоги! Участники:";
        public static string GameDidntActivateStr { get; } = "Вы не активировали игру 'Время шаурмы'!";
        public static string NotGameTimeStr { get; } = $"Еще не 'Время шаурмы'!\r\nКаждый {DayOfWeek} в {StartHour.ToString().PadLeft(2, '0')}:{StartMinute.ToString().PadLeft(2, '0')}.";
        #endregion
    }
    #endregion
}
