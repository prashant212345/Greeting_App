using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.GlobalExceptionHandler
{
    public class ExceptionHandler : Exception
    {
        public int StatusCode { get; }
        public ExceptionHandler(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
