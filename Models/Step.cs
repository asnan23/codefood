using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models
{
    public class Step
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int recipeId { get; set; }
        [Required]
        public int stepOrder { get; set; }
        public string description { get; set; }
    }
}
