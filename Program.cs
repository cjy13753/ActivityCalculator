public class CalendarQuickstart
{
    static void Main(string[] args)
    {
        //var targetDate = new DateTime(2023, 5, 20);
        var targetDate = DateTime.Today.AddDays(-1);   
        
        var relevantEvents = EventsFetcher.FetchRelevant(targetDate);
        
        EventConsolePrinter.PrintHoursSpentForOnlyWorkingActivtyTypes(relevantEvents);
        Console.WriteLine("==========");
        EventConsolePrinter.PrintHoursSpentForOnlyNonWorkingActivtyTypes(relevantEvents);
        Console.WriteLine("==========");
        EventConsolePrinter.PrintAllEvents(relevantEvents);
    }
}