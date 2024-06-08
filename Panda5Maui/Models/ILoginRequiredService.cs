using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda5Maui.Models;

interface ILoginRequiredService
{
    /// <summary>Makes a login request to underlying service</summary>
    /// <returns>Login errors if they have been encountered</returns>
    public Task<string?> LoginAsync(string email, string password);

    /// <summary>Makes a Register request to underlying service</summary>
    /// <returns>Register errors if they have been encountered</returns>
    public Task<string?> RegisterAsync(string username, string password, string email);
}
