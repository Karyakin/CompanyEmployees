using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Filters.TestsFlters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult
            {
                Content = $"В классе {this} возникло исключение {context.Exception.Message}" +
                $"{context.Exception.Data}, {context.ActionDescriptor.DisplayName}"
            };
            context.ExceptionHandled = true;
        }
    }
}
