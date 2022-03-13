using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.ViewModel
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "username is required")]
        [EmailAddress]
        public string username { get; set; }
        [Required(ErrorMessage = "password is required")]
        [MinLength(6,ErrorMessage = "password minimum 6 characters")]
        public string password { get; set; }
    }
}
