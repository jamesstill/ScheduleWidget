namespace ScheduleWidget.Enums
{
    /// <summary>
    /// The frequency at which an event repeats - every Day, Week, Month, Quarter, Year etc. 
    /// Cannot be combined like Monthly, Daily etc. (XOR type)
    /// </summary>
    public enum FrequencyTypeEnum
    {
        None = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 4,
        Quarterly = 8,
        Yearly = 16
    }
}
