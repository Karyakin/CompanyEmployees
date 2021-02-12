using CompanyEmployees.Filters.TestsFlters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers.TestControllers
{
    //[MyAddHeader("Author", "Rick Anderson")]
    public class SampleController : CustomController
    {
       // [HttpGet]
        //[TypeFilter(typeof(MyActionFilter))]
        [SimpleResourceFilterParametr(30, "Дмитрон")]// фильтр с параметрами
        public IActionResult Index()
        {
            return Ok("all right");
        }

        //  [SimpleResourceFilter] //фильтр добавляет Куки
        //[ServiceFilter(typeof(LoggerResourceFilter))]// если применяется такая конструкция, то нужно регистрировать в стартапе
        // [TypeFilter(typeof(LoggerResourceFilter))]//// если применяется такая конструкция, то НЕ нужно регистрировать в стартапе
        [HttpGet]
        [CustomExceptionFilter]
        public IActionResult Index2()
        {
            int a = 0;
            int b = 1;
            int c = b / a;
            return Content("Header values by configuration.");
        }
    }
}
