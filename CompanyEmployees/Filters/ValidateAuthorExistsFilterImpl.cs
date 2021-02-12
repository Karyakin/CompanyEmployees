using Conrtacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Filters
{
    /// <summary>
    /// каласс сборка в котором описывается логика проверки
    /// созданный мной
    /// </summary>
    public class ValidateAuthorExistsFilterImpl : IAsyncActionFilter
    {
        private readonly IEmployeeRepository _authorRepository;

        public ValidateAuthorExistsFilterImpl(IEmployeeRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = context.ActionArguments["id"] as Guid?;
                if (id.HasValue)
                {
                    if (( _authorRepository.GetEmployeeSortByAge(false)).All(a => a.Id != id.Value))
                    {
                        context.Result = new NotFoundObjectResult(
                          id.Value);
                        return;
                    }
                }
            }
            await next();
        }
    }
}
