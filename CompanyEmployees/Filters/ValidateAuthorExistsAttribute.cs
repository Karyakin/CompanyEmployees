using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Filters
{
    public class ValidateAuthorExistsAttribute : TypeFilterAttribute
    {
        public ValidateAuthorExistsAttribute():base(typeof(ValidateAuthorExistsFilterImpl))
        {

        }
    }
}
