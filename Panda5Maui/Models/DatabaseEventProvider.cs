using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Panda5Maui.Models;

internal class DatabaseEventProvider : IEventProvider
{
    private IDatabaseProvider DatabaseProvider;
    private List<CalendarDayEvent> CalendarEvents = [];

    public event Action<List<CalendarDayEvent>> CalendarEventsUpdated;

    public DatabaseEventProvider(IDatabaseProvider databaseProvider)
    {
        DatabaseProvider = databaseProvider;
        GetEvents().ContinueWith(_ => InvokeCalendarEventsUpdated());
    }

    public async Task AddEvent(CalendarDayEvent calendarEvent)
    {
        CalendarEvents.Add(calendarEvent);
        if (!await UpdateDatabase()) CalendarEvents.Remove(calendarEvent);
        else InvokeCalendarEventsUpdated();
    }

    public async Task RemoveEvent(CalendarDayEvent calendarEvent)
    {
        CalendarEvents.Remove(calendarEvent);
        if (!await UpdateDatabase()) CalendarEvents.Add(calendarEvent);
        else InvokeCalendarEventsUpdated();
    }

    private async Task GetEvents()
    {
        Dictionary<string, object> userData = await DatabaseProvider.GetUserdata();
        if (userData == null) return;
        foreach (object dataObject in userData.Values.ToArray())
        {
            CalendarDayEvent? calendarEvent = JsonSerializer.Deserialize<CalendarDayEvent>((string)dataObject);
            if (calendarEvent != null) CalendarEvents.Add(calendarEvent);
        }
    }

    private async Task<bool> UpdateDatabase()
    {
        Dictionary<string, object> newUserData = new();
        for (int i = 0; i < CalendarEvents.Count; i++)
        {
            newUserData.Add(i.ToString(), JsonSerializer.Serialize(CalendarEvents[i]));
        }
        return await DatabaseProvider.SetUserdata(newUserData);
    }

    private void InvokeCalendarEventsUpdated()
    {
        Application.Current?.Dispatcher.Dispatch(() =>
        {
            CalendarEventsUpdated?.Invoke(CalendarEvents);
        });
    }
}
