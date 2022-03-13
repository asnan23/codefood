using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "username is required")]
        [EmailAddress]
        public string username { get; set; }
        [Required(ErrorMessage = "password is required")]
        public string password { get; set; }
    }
}
