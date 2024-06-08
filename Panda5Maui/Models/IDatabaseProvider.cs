using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda5Maui.Models;

internal interface IDatabaseProvider
{
    public Task<Dictionary<string, object>> GetUserdata();
    public Task<bool> SetUserdata(Dictionary<string, object> userdata);
}
