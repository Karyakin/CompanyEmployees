using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Dto_DataTransferObjects__
{
    public class EmployeeForCreationDto : EmployeeForManipulationDto
    {
        [MaxLength(40, ErrorMessage = "Maximum length for the Name is 10 characters.")]
        [Required(ErrorMessage = "Employee name is a required field.")]
        public string CompanyName { get; set; }

      /*  *//*    [MaxLength(10, ErrorMessage = "Maximum length for the Name is 10 characters.")]*//*
        [Required(ErrorMessage = "Employee name is a required field.")]
        public string Name { get; set; }

        [Range(18, int.MaxValue, ErrorMessage = "Возраст не может быть меньше 18")]
        [Required(ErrorMessage = "Position is a required field.")]
        public int Age { get; set; }
        public string Position { get; set; }*/

    }
}
