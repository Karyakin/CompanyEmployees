using AutoMapper;
using Conrtacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Filters
{
    public class UserExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        public readonly IRepositoryWrapper _wrapper;

        public UserExistsAttribute(IRepositoryWrapper wrapper, ILoggerManager  logger, IMapper mapper)
        {
            _logger = logger;
            _wrapper = wrapper;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT"); 
            var id = (Guid)context.ActionArguments["id"];
            var company =  _wrapper.Company.GetCompanyAsync(id, trackChanges);

            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database."); 
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("company", company); 
                await next();
            }
        }




    


}
}
