using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Panda5Maui.Converters;

namespace Panda5Maui.Models;

internal class CalendarDayEvent : XCalendar.Core.Models.Event
{
    private Color _color = Color.FromRgb(255, 255, 255);
    [JsonConverter(typeof(JsonColorConverter))]
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged();
        }
    }
}
