namespace DeliveryCorporationBot.Constants
{
    #region EventTimeConstants
    public static class EventTimeConstants
    {
        #region static commands
        public static string EventTimeCommandStr { get; } = "eventtime";
        public static string EventTimeCommandDescrStr { get; } = "Провести конкурс";
        public static string StartEventTimeCommandStr { get; } = "startEventTime";
        public static string StartEventTimeCommandDescrStr(int number) => $"Выбрать {number}";
        public static string WantEventTimeCommandStr { get; } = "wantEventTime";
        public static string FinishEventTimeCommandDescrStr { get; } = "Подвести итоги";
        public static string FinishEventTimeCommandStr { get; } = "finishEventTime";
        #endregion
        #region static strings
        public static string ChooseParticipientNumberStr { get; } = "Укажите количество участников";
        public static string MinParticipientNumberStr { get; } = "Минимальное кол-во участников - 1";
        public static string StartedEventTime { get; } = "Желающие поучавствовать?";
        public static string ResultStr { get; } = "Участниками становятся:\r\n";
        #endregion
    }
    #endregion
}
