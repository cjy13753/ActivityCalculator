using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

public class EventsValidator
{
    public void CheckIfNoOverlappingEvents(IList<Event> eventItems)
    {
        for (int i = 1; i < eventItems.Count; i++)
        {
            var prevEventEndTime = eventItems[i - 1].End.DateTime;
            var currentEventStartTime = eventItems[i].Start.DateTime;
            if (prevEventEndTime <= currentEventStartTime)
            {
                continue;
            }
            throw new Exception("Check your events. No overlapping events can exist.");
        }
    }
}

public class EventsFetcher
{
    private readonly EventsValidator _eventsValidator;

    public EventsFetcher()
    {
        _eventsValidator = new EventsValidator();
    }

    public List<Event> FetchRelevant(DateTime targetDate)
    {
        var events = FetchAll(targetDate);
        return FilterRelevantEvents(events);
    }

    private IList<Event> FetchAll(DateTime targetDate)
    {
        var configuration = GetConfiguration();
        var service = GetCalendarService(configuration);
        return GetEvents(configuration, service, targetDate);
    }

    private List<Event> FilterRelevantEvents(IList<Event> events)
    {
        var validColorIds = ColorIdProvider.GetAllRelevantColorIdInfo().Select(info => info.Number).ToList();
        var filter = 0;
        var todayEvents = new List<Event>();

        // 1. Remove any events preceding the first occurence of a "취침" event.
        // 2. Remove all events commencing with, and subsequent to, the second occurrence of a "취침" event.
        foreach (var item in events)
        {
            if (item.Summary == "취침")
            {
                filter++;
            }

            if (filter != 1)
            {
                continue;
            }

            if (validColorIds.Contains(item.ColorId))
            {
                todayEvents.Add(item);
            }
        }

        // Retain only the consective events because the non-consecutive events are not what actually happened, but planned.
        var consecutiveEvents = new List<Event>
        {
            todayEvents[0]
        };
        for (int i = 1; i < todayEvents.Count; i++)
        {
            if (todayEvents[i].Start.DateTime != todayEvents[i - 1].End.DateTime)
            {
                break;
            }
            consecutiveEvents.Add(todayEvents[i]);
        }

        return consecutiveEvents;
    }

    private IList<Event> GetEvents(IConfigurationRoot configuration, CalendarService service, DateTime targetDate)
    {
        var timeMin = targetDate;
        var timeMax = targetDate.AddDays(1);

        EventsResource.ListRequest request = service.Events.List(configuration["CalendarId"]);
        request.TimeMin = timeMin;
        request.TimeMax = timeMax;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        Events events = request.Execute();
        var eventsItems = events.Items;

        _eventsValidator.CheckIfNoOverlappingEvents(eventsItems);
        FillNullColorIdsOfEventsWithDefaultColor(eventsItems);

        return events.Items;
    }
    private void FillNullColorIdsOfEventsWithDefaultColor(IList<Event> eventsItems)
    {
        foreach (var item in eventsItems)
        {
            item.ColorId ??= ColorIdProvider.GetDefaultCalendarColorIdInfo().Number;
        }
    }

    private CalendarService GetCalendarService(IConfigurationRoot configuration)
    {
        UserCredential credential;
        using (var stream = new FileStream(configuration["CredentialPath"], FileMode.Open, FileAccess.Read))
        {
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new[] { CalendarService.Scope.CalendarReadonly },
                "user", CancellationToken.None).Result;
        }

        var service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Calendar API Sample",
        });
        return service;
    }

    private IConfigurationRoot GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false);
        var configuration = builder.Build();
        return configuration;
    }
}
