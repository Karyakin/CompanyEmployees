using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Dto_DataTransferObjects__
{
    /*Чтобы вернуть 422 вместо 400, первое, что нам нужно сделать, это подавить BadRequest ошибка, когда ModelState является недействительным. Мы собираемся сделать это, добавив этот код в Startup класс в ConfigureServices метод:

    services.Configure<ApiBehaviorOptions>(options =>
    {
    options.SuppressModelStateInvalidFilter = true;
    });
    */
    public class CompanyForCreationDto
    {
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Country { get; set; }
    }
}
