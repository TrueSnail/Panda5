using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XCalendar.Core.Models;

namespace Panda5Maui.ViewModels;

internal class CalendarViewModel : ObservableObject
{
    public Calendar<CalendarDay> Calendar { get; set; } = new Calendar<CalendarDay>()
    {
        SelectionAction = XCalendar.Core.Enums.SelectionAction.Modify,
        SelectionType = XCalendar.Core.Enums.SelectionType.Single
    };

    public Command DaySelect { get; }

    public CalendarViewModel()
    {
        DaySelect = new Command<DateTime>((Day) =>
        {
            Calendar.SelectedDates.Clear();
            Calendar.NavigatedDate = Day;
            Calendar.SelectedDates.Add(Day);
        });
    }
}
