using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCalendar.Core.Models;

namespace Panda5Maui.ViewModels;

internal class CalendarViewModel : ObservableObject
{
    public Calendar<CalendarDay> Calendar { get; set; } = new Calendar<CalendarDay>();

    public CalendarViewModel()
    {
    }
}
