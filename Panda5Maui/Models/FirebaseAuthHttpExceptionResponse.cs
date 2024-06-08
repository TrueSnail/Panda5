using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panda5Maui.Models;

internal class FirebaseAuthHttpExceptionResponse
{
    public Error error { get; set; }

    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<ErrorList> errors { get; set; }
    }

    public class ErrorList
    {
        public string message { get; set; }
        public string domain { get; set; }
        public string reason { get; set; }
    }
}
