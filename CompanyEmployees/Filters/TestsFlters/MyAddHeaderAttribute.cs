using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Filters.TestsFlters
{
    public class MyAddHeaderAttribute : ResultFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;

        /// <summary>
        /// Атрибуты позволяют фильтрам принимать аргументы, как показано в примере выше "string name, string value"
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public MyAddHeaderAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(_name, new string[] { _value });
            base.OnResultExecuting(context);
        }


    }
}
