namespace Panda5Maui;
using Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(CalendarView), typeof(CalendarView));
    }
}