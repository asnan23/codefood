using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models
{
    public class Ingredient
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int recipeId { get; set; }
        [Required]
        public string item { get; set; }
        [Required]
        public string unit { get; set; }
        [Required]
        public int value { get; set; }
    }
}
