using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda5Maui.Converters;

internal class FirstCharToUpperConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string text || text.Length < 1) return string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsLetter(text[i])) return text.Substring(0, i) + char.ToUpper(text[i]) + text.Substring(i + 1);
        }
        return text;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
