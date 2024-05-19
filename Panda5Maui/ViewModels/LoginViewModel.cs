using CommunityToolkit.Mvvm.ComponentModel;
using Panda5Maui.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda5Maui.ViewModels;

internal class LoginViewModel : ObservableObject
{
    public string Username => "test";
    public Command ButtonPressCommand { get; }


    public LoginViewModel()
    {
        ButtonPressCommand = new Command(async () =>
        {
            OnPropertyChanged(nameof(Username));
            await Shell.Current.GoToAsync(nameof(CalendarView));
        });
    }
}
