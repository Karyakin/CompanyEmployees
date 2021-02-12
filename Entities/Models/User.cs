using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "User name is a required field.")]
        [MaxLength(5, ErrorMessage = "Maximum length for the Name is 5 characters.")]
        public string UserName { get; set; }

        [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
        public int Age { get; set; }

        [Required(ErrorMessage = "User name is a required field.")]
        [MaxLength(5, ErrorMessage = "Maximum length for the Name is 5 characters.")]
        public string Role { get; set; }
    }
}
