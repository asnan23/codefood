using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class IngredientDto
    {
        [Required(ErrorMessage = "Item is required")]
        public string item { get; set; }
        [Required(ErrorMessage = "Unit is required")]
        public string unit { get; set; }
        [Required(ErrorMessage = "Value is required")]
        public int value { get; set; }
    }
}
