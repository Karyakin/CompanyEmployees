using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Dto_DataTransferObjects__
{
    public class EmployeeForUpdateDto : EmployeeForManipulationDto
    {
      /*  [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(10, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(10, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Position { get; set; }*/
    }

}
