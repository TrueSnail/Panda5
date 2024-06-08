using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Core;
using Panda5Maui.Models;
using Panda5Maui.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panda5Maui.ViewModels;

internal partial class LoginViewModel : ObservableObject
{
    private ILoginRequiredService LoginService;
    private bool WaitingForVerification = false;

    public Command RegisterButtonCommand { get; }
    public Command LoginButtonCommand { get; }
    public Command VerifyEmailCommand { get; }
    public Command VerifyPasswordCommand { get; }

    public string EmailInput { get; set; } = "";
    public string PasswordInput { get; set; } = "";
    [ObservableProperty]
    private string loginError = "";
    [ObservableProperty]
    private string verifyEmailError = "";
    [ObservableProperty]
    private string verifyPasswordError = "";

    public LoginViewModel(ILoginRequiredService loginService)
    {
        LoginService = loginService;

        LoginButtonCommand = new Command(LoginButtonPress);
        RegisterButtonCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(RegisterView)));

        VerifyPasswordCommand = new Command<FocusEventArgs>((FocusEventArgs e) => 
        {
            bool verified = Verifiers.VerifyPassword(PasswordInput);
            Verifiers.UpdateEntry((Entry)e.VisualElement, e.IsFocused, verified);
            VerifyPasswordError = verified || e.IsFocused ? "" : "Password must have at least 6 characters";
        });

        VerifyEmailCommand = new Command<FocusEventArgs>((FocusEventArgs e) =>
        {
            bool verified = Verifiers.VerifyEmail(EmailInput);
            Verifiers.UpdateEntry((Entry)e.VisualElement, e.IsFocused, verified);
            VerifyEmailError = verified || e.IsFocused ? "" : "This is not a valid email adress";
        });
    }

    private async void LoginButtonPress()
    {
        if (!Verifiers.VerifyEmail(EmailInput) || !Verifiers.VerifyPassword(PasswordInput))
        {
            LoginError = "The fields above are not completed correctly";
            return;
        }
        else if (WaitingForVerification) return;

        WaitingForVerification = true;
        LoginError = "...";
        string? result = await LoginService.LoginAsync(EmailInput, PasswordInput);

        if (result == null) await Shell.Current.GoToAsync(nameof(CalendarView));
        else LoginError = result;

        WaitingForVerification = false;
    }
}
