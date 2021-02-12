using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Filters.TestsFlters
{
    public class SimpleResourceFilterParametr : Attribute, IResourceFilter
    {
        public int _id { get; }
        public string _token { get; }

        public SimpleResourceFilterParametr(int id, string token)
        {
            _id = id;
            _token = token;
        }



        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("id", _id.ToString());
            context.HttpContext.Response.Headers.Add("token", _token.ToString());
        }


        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            context.Result = new ContentResult { Content = "Ресурс не найден" };
        }
    }
}
