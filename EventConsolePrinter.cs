﻿using Google.Apis.Calendar.v3.Data;

public static class EventConsolePrinter
{
    public static void PrintAllEvents(List<Event> validEvents)
    {
        foreach (var item in validEvents)
        {
            var start = item.Start.DateTime.ToString();
            var end = item.End.DateTime.ToString();

            Console.WriteLine($"시작: {start} ~ 끝: {end} - {item.Summary}");
        }
    }

    public static void PrintHoursSpentForWorkingActivities(List<Event> relevantEvents)
    {
        var workingColorIdInfo = ColorIdProvider.GetOnlyWorkingColorIds();

        foreach (var info in workingColorIdInfo)
        {
            var particularActivity = relevantEvents.Where(e => e.ColorId == info.Number).ToList();
            var timeSpent = particularActivity.Select(e => (e.End.DateTime - e.Start.DateTime)).Select(e => e.Value).Aggregate(TimeSpan.Zero, (subtotal, timeSpan) => subtotal + timeSpan);
            Console.WriteLine($"{info.Name} Time Spent: {timeSpent.TotalHours} hours");
        }
    }
}