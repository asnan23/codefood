using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "name is required")]
        public string name { get; set; }
    }
}
