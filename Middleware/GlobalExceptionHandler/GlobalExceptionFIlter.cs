using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;

namespace Middleware.GlobalExceptionHandler
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext context)
        {
            string errorMessage = context.Exception.Message;
            int statusCode = 500;

            if(context.Exception is ExceptionHandler customException)
            {
                statusCode = customException.StatusCode;
            }

            Logger.Error("Exception: " + errorMessage);

            var response = new
            {
                Success = false,
                Message = errorMessage,
                StatusCode = statusCode
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
