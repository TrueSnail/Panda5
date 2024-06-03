using CommunityToolkit.Mvvm.ComponentModel;
using Panda5Maui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XCalendar.Core.Models;

namespace Panda5Maui.ViewModels;

internal class CalendarViewModel : ObservableObject
{
    public Calendar<CalendarDay<CalendarDayEvent>, CalendarDayEvent> Calendar { get; } = new()
    {
        SelectionAction = XCalendar.Core.Enums.SelectionAction.Modify,
        SelectionType = XCalendar.Core.Enums.SelectionType.Single
    };

    public Command DaySelectCommand { get; }
    public Command ChangeMonthCommand { get; }
    public Command NewEventButtonCommand { get; }
    public Command DeleteEventButtonCommand { get; }

    public List<string> NewEventColors { get; } =
    [
        "Red",
        "Green",
        "Blue",
        "Cyan",
        "Magenta",
        "Yellow",
        "White"
    ];
    public string NewEventTitle { get; set; } = "";
    public string NewEventDescription { get; set; } = "";
    public string NewEventColor { get; set; }
    public string NewEventErrorText { get; private set; } = "";
    public ObservableCollection<CalendarDayEvent> SelectedDayEvents { get; private set; } = [];
    public string SelectedDateString => Calendar.SelectedDates.Count > 0 ? Calendar.SelectedDates[0].ToString("dd MMMM yyyy").TrimStart('0') : "No date was selected ¯\\_(ツ)_/¯";

    public CalendarViewModel()
    {
        NewEventColor = NewEventColors[0];

        DaySelectCommand = new Command<DateTime>(DaySelect);
        ChangeMonthCommand = new Command<int>(ChangeMonth);
        NewEventButtonCommand = new Command(NewEventButton);
        DeleteEventButtonCommand = new Command<CalendarDayEvent>(DeleteEventButton);

        Calendar.DateSelectionChanged += Calendar_DateSelectionChanged;
    }

    private void Calendar_DateSelectionChanged(object? sender, DateSelectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SelectedDateString));
        UpdateSelectedDayEvents();
    }

    private void UpdateSelectedDayEvents()
    {
        if (Calendar.SelectedDates.Count == 0)
        {
            SelectedDayEvents.Clear();
            return;
        }

        SelectedDayEvents.Clear();
        List<CalendarDayEvent> selectedDayEvents = new(Calendar.Events.Where(dayEvent => Calendar.SelectedDates[0] >= dayEvent.StartDate && dayEvent.EndDate > Calendar.SelectedDates[0]));
        foreach (var item in selectedDayEvents) SelectedDayEvents.Add(item);

    }

    private void NewEventButton()
    {
        if (Calendar.SelectedDates.Count < 1) NewEventErrorText = "No date selected";
        else if (NewEventTitle.Trim().Length < 1) NewEventErrorText = "Plese fill event title";
        else
        {
            Calendar.Events.Add(new CalendarDayEvent()
            {
                Title = NewEventTitle,
                Description = NewEventDescription,
                StartDate = Calendar.SelectedDates[0],
                EndDate = Calendar.SelectedDates[0].AddDays(1),
                Color = Color.Parse(NewEventColor)
            });
            NewEventErrorText = "";
            UpdateSelectedDayEvents();
        }
        OnPropertyChanged(nameof(NewEventErrorText));
    }

    private void DeleteEventButton(CalendarDayEvent calendarDayEvent)
    {
        Calendar.Events.Remove(calendarDayEvent);
        UpdateSelectedDayEvents();
    }

    private void DaySelect(DateTime day)
    {
        Calendar.SelectedDates.Clear();
        Calendar.NavigatedDate = day;
        Calendar.SelectedDates.Add(day);
    }

    private void ChangeMonth(int monthOffset)
    {
        Calendar.Navigate(Calendar.NavigatedDate.AddMonths(monthOffset) - Calendar.NavigatedDate);
    }

    //CalendarEventHandler(List<IEventProvider>) / DatabaseEventProvider(IDatabaseProvider) : IEventProvider / FirebaseDatabaseProvider() : IDatabaseProvider, IAccountProvider
}
