namespace ProjectTasksTrackService.Core.Enums
{
    /// <summary>
    /// Повторяемость задачи/напоминания
    /// </summary>
    public enum TaskRepeatsType : short
    {
        /// <summary> Заблокированная задача (выключенное напоминание) </summary>
        BlockedOrDisabled = -1,

        /// <summary> Однократно (без повторов) </summary>
        OneTimeTaskEvent = 0,

        /// <summary> Повторять в первый день месяца </summary>
        RepeatAtFirstDayOfMonth = 1,

        /// <summary> Повторять 1 раз в месяц (по номеру дня) </summary>
        RepeatMonthlyAtDate = 30,

        /// <summary> Повторять в последний день месяца </summary>
        RepeatAtLastDayOfMonth = 31,

        /// <summary> Повторять через заданное количество дней </summary>
        RepeatInDays = 90
    }
}
