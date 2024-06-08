using CommunityToolkit.Mvvm.ComponentModel;
using Panda5Maui.Models;
using Panda5Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda5Maui.ViewModels;

internal partial class RegisterViewModel : ObservableObject
{
    private ILoginRequiredService LoginService;
    private bool WaitingForVerification = false;

    public Command VerifyConfirmPasswordCommand { get; }
    public Command VerifyEmailCommand { get; }
    public Command VerifyUsernameCommand { get; }
    public Command VerifyPasswordCommand { get; }
    public Command RegisterButtonCommand { get; }

    public string UsernameInput { get; set; } = "";
    public string EmailInput { get; set; } = "";
    public string PasswordInput { get; set; } = "";
    public string ConfirmPasswordInput { get; set; } = "";
    [ObservableProperty]
    private string verifyUsernameError = "";
    [ObservableProperty]
    private string verifyEmailError = "";
    [ObservableProperty]
    private string verifyPasswordError = "";
    [ObservableProperty]
    private string verifyConfirmPasswordError = "";
    [ObservableProperty]
    private string registerError = "";

    public RegisterViewModel(ILoginRequiredService loginService)
    {
        LoginService = loginService;

        VerifyPasswordCommand = new Command<FocusEventArgs>((FocusEventArgs e) =>
        {
            bool verified = Verifiers.VerifyPassword(PasswordInput);
            Verifiers.UpdateEntry((Entry)e.VisualElement, e.IsFocused, verified);
            VerifyPasswordError = verified || e.IsFocused ? "" : "Password must have at least 6 characters";
        });

        VerifyConfirmPasswordCommand = new Command<FocusEventArgs>((FocusEventArgs e) =>
        {
            bool verified = Verifiers.VerifyConfirmPassword(PasswordInput, ConfirmPasswordInput);
            Verifiers.UpdateEntry((Entry)e.VisualElement, e.IsFocused, verified);
            VerifyConfirmPasswordError = verified || e.IsFocused ? "" : "Password is less than 6 characters or does not match";
        });

        VerifyUsernameCommand = new Command<FocusEventArgs>((FocusEventArgs e) =>
        {
            bool verified = Verifiers.VerifyUsername(UsernameInput);
            Verifiers.UpdateEntry((Entry)e.VisualElement, e.IsFocused, verified);
            VerifyUsernameError = verified || e.IsFocused ? "" : "Username must by at lest 2 characters long";
        });

        VerifyEmailCommand = new Command<FocusEventArgs>((FocusEventArgs e) =>
        {
            bool verified = Verifiers.VerifyEmail(EmailInput);
            Verifiers.UpdateEntry((Entry)e.VisualElement, e.IsFocused, verified);
            VerifyEmailError = verified || e.IsFocused ? "" : "This is not a valid email adress";
        });

        RegisterButtonCommand = new Command(RegisterButtonPress);
    }

    private async void RegisterButtonPress()
    {
        if (!Verifiers.VerifyEmail(EmailInput) || 
            !Verifiers.VerifyPassword(PasswordInput) ||
            !Verifiers.VerifyUsername(UsernameInput) ||
            !Verifiers.VerifyConfirmPassword(PasswordInput, ConfirmPasswordInput))
        {
            RegisterError = "The fields above are not completed correctly";
            return;
        }
        else if (WaitingForVerification) return;

        WaitingForVerification = true;
        RegisterError = "...";
        string? result = await LoginService.RegisterAsync(UsernameInput, PasswordInput, EmailInput);

        if (result == null) await Shell.Current.GoToAsync(nameof(CalendarView));
        else RegisterError = result;

        WaitingForVerification = false;
    }
}
