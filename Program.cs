public class CalendarQuickstart
{
    static void Main(string[] args)
    {
        var targetDate = new DateTime(2023, 5, 20);
        
        var relevantEvents = EventsFetcher.FetchRelevant(targetDate);
        
        EventConsolePrinter.PrintHoursSpentForWorkingActivities(relevantEvents);
        Console.WriteLine("==========");
        EventConsolePrinter.PrintAllEvents(relevantEvents);
    }
}