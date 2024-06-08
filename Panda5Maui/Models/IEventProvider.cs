using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda5Maui.Models;

internal interface IEventProvider
{
    public event Action<List<CalendarDayEvent>> CalendarEventsUpdated;
    public Task AddEvent(CalendarDayEvent calendarEvent);
    public Task RemoveEvent(CalendarDayEvent calendarEvent);
}
