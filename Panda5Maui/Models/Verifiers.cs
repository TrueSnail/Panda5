using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panda5Maui.Models;

internal static class Verifiers
{
    public static void UpdateEntry(Entry entry, bool? isFocused, bool isVerified)
    {
        if (isVerified || (isFocused ?? false)) entry.TextColor = Color.FromRgb(255, 255, 255);
        else entry.TextColor = Color.FromRgb(255, 95, 95);
    }

    public static bool VerifyEmail(string email)
    {
        Regex regex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        return regex.IsMatch(email);
    }

    public static bool VerifyPassword(string password) => password.Length > 5;

    public static bool VerifyConfirmPassword(string password, string confirmPassword) => VerifyPassword(confirmPassword) && password == confirmPassword;

    public static bool VerifyUsername(string username) => username.Length > 2;
}

